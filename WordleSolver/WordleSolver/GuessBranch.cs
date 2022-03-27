using System.Collections.Generic;

namespace WordleSolver
{
    /// <summary>
    /// A branch of results.
    /// </summary>
    public sealed class GuessBranch
    {
        /// <summary>
        /// The clues pattern that leads to these results.
        /// </summary>
        public Clues Clues { get; }

        /// <summary>
        /// The potential target words in this branch.
        /// </summary>
        public List<string> Words { get; } = new List<string>();

        /// <summary>
        /// Creates a branch of guess results for the specified <paramref name="clues"/> pattern.
        /// </summary>
        /// <param name="clues">The clues pattern that leads to these results.</param>
        public GuessBranch(Clues clues)
        {
            Clues = clues;
        }
    }
}
