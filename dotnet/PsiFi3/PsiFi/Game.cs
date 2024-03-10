namespace PsiFi
{
    /// <summary>
    /// A game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Creates a new <see cref="Game"/>.
        /// </summary>
        /// <param name="protagonist">This game's protagonist.</param>
        public Game(Protagonist protagonist)
        {
            Protagonist = protagonist;
        }

        /// <summary>
        /// This game's protagonist.
        /// </summary>
        public Protagonist Protagonist { get; }

        /// <summary>
        /// This game's current mission.
        /// </summary>
        public Mission? Mission { get; internal set; }
    }
}
