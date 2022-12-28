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
        public GameEngine(Protagonist protagonist)
        {
            Protagonist = protagonist;
        }

        /// <summary>
        /// This game's protagonist.
        /// </summary>
        public Protagonist Protagonist { get; }

        /// <summary>
        /// Runs a game.
        /// </summary>
        /// <returns>A sequence of interactions.</returns>
        public IEnumerable<Interaction> RunGame()
        {
            var missionEngine = new MissionEngine(Protagonist);
            foreach (var interaction in missionEngine.RunMission()) yield return interaction;
        }
    }
}
