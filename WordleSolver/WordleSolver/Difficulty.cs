namespace WordleSolver
{
    /// <summary>
    /// The player's selected difficulty.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// Each guess can be any valid word.
        /// </summary>
        Normal,

        /// <summary>
        /// Each guess must include all previous clues.
        /// </summary>
        Hard,
    }
}