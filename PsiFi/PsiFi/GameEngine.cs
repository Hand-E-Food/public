using PsiFi.Interactions;

namespace PsiFi
{
    /// <summary>
    /// Manages playing a game.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        /// Creates a new <see cref="GameEngine"/>.
        /// </summary>
        /// <param name="protagonist">This game's protagonist.</param>
        public GameEngine(Game game)
        {
            Game = game;
        }

        /// <summary>
        /// The game being run by this engine.
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// Runs a game.
        /// </summary>
        /// <returns>A sequence of interactions.</returns>
        public IEnumerable<Interaction> RunGame()
        {
            var mapGenerator = new MapGenerator();
            Game.Mission = new Mission(mapGenerator.CreateMap());
            var missionEngine = new MissionEngine(Game);
            yield return new StartMissionInteraction(missionEngine.RunMission());
            yield return new EndMissionInteraction();
        }
    }
}
