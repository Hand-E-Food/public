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
        public int Clues { get; }

        /// <summary>
        /// The potential target words in this branch.
        /// </summary>
        public List<int> WordIndices { get; }

        /// <summary>
        /// Creates a branch of guess results for the specified <paramref name="clues"/> pattern.
        /// </summary>
        /// <param name="clues">The clues pattern that leads to these results.</param>
        /// <param name="capacity">The capacity to assign to <see cref="WordIndices"/>.</param>
        public GuessBranch(int clues, int capacity)
        {
            Clues = clues;
            WordIndices = new(capacity);
        }
    }
}
