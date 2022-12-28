using PsiFi.Abilities;
using PsiFi.Interactions;

namespace PsiFi
{
    internal class MissionEngine
    {
        private readonly ICollection<Ability> abilities;
        private readonly QuitAbility quitAbility = new();

        /// <summary>
        /// Creates a new <see cref="MissionEngine"/>.
        /// </summary>
        /// <param name="protagonist">This mission's protagonist.</param>
        public MissionEngine(Protagonist protagonist)
        {
            Protagonist = protagonist;
            abilities = Protagonist.Abilities
                .Concat(Protagonist.Skills
                    .Select(skill => skill.Ability)
                    .WhereNotNull()
                )
                .Append(quitAbility)
                .ToArray();
        }

        /// <summary>
        /// This mission's protagonist.
        /// </summary>
        public Protagonist Protagonist { get; }

        /// <summary>
        /// Runs a mission.
        /// </summary>
        /// <returns>A sequence of interactions.</returns>
        public IEnumerable<Interaction> RunMission()
        {
            yield return new StartMissionInteraction();
            var room = new MapGenerator().CreateMap();
            Protagonist.Room = room;
            yield return new ShowRoomInteraction(room);
            while (!quitAbility.Activated && Protagonist.Room != null)
            {
                foreach (var interaction in RunProtagonist()) yield return interaction;
            }
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
