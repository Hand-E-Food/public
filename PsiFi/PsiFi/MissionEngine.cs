using PsiFi.Interactions;
using System.Linq;

namespace PsiFi
{
    internal class MissionEngine
    {
        private readonly ICollection<Ability> abilities;

        /// <summary>
        /// Creates a new <see cref="MissionEngine"/>.
        /// </summary>
        /// <param name="game">The game to run.</param>
        public MissionEngine(Game game)
        {
            Game = game;
            if (game.Protagonist == null) throw new InvalidOperationException("The game must have a protagonist.");
            if (game.Mission == null) throw new InvalidOperationException("The game must have a current mission.");
            abilities = Game.Protagonist.Abilities
                .Concat(Game.Protagonist.Skills
                    .Select(skill => skill.Ability)
                    .WhereNotNull()
                )
                .Append(game.Mission.QuitAbility)
                .ToArray();
        }

        /// <summary>
        /// The game.
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// The game's current mission.
        /// </summary>
        private Mission Mission => Game.Mission!;

        /// <summary>
        /// The game's protagonist.
        /// </summary>
        private Protagonist Protagonist => Game.Protagonist!;

        /// <summary>
        /// Runs a mission.
        /// </summary>
        /// <returns>A sequence of interactions.</returns>
        public IEnumerable<Interaction> RunMission()
        {
            yield return MoveProtagonistToRoom(Mission.EntryRoom);
            while (!Mission.QuitAbility.Activated && Game.Protagonist.Room != null)
            {
                foreach (var interaction in RunProtagonist()) yield return interaction;
            }
        }

        /// <summary>
        /// Moves the protagonist to the specified room.
        /// </summary>
        /// <param name="room">The room to move to.</param>
        /// <returns>The interaction that displays the new room.</returns>
        private ShowRoomInteraction MoveProtagonistToRoom(Room room)
        {
            Protagonist.Room = room;
            return new ShowRoomInteraction(room);
        }

        /// <summary>
        /// Runs the protagonist's turn.
        /// </summary>
        /// <returns>A sequence of interactions.</returns>
        private IEnumerable<Interaction> RunProtagonist()
        {
            var request = new SelectionInteraction<Ability>(abilities.Where(ability => ability.CanActivateAbility));
            do yield return request;
            while (request.Response == null);
            foreach (var interaction in request.Response.ActivateAbility()) yield return interaction;
        }
    }
}
