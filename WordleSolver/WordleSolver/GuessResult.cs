using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WordleSolver
{
    public sealed class GuessResult : IComparable<GuessResult>, IEnumerable<GuessBranch>
    {
        private readonly GuessBranch[] branches;

        /// <summary>
        /// Gets the branch for the specified <paramref name="clues"/> pattern.
        /// </summary>
        /// <param name="clues">The clues pattern of the branch to get.</param>
        /// <returns>The branch for the specified <paramref name="clues"/> pattern.</returns>
        public GuessBranch this[int clues] => branches[clues];

        /// <summary>
        /// The word that was guessed.
        /// </summary>
        public int GuessWordIndex { get; }

        /// <summary>
        /// The count of the largest branch.
        /// </summary>
        public int LargestCount => branches.Max(branch => branch.WordIndices.Count);

        /// <summary>
        /// Creates an object to store the results of a guess.
        /// </summary>
        /// <param name="wordIndex">The word to guess.</param>
        /// <param name="depth">The depth of this guess, starting from 1.</param>
        /// <param name="capacity">The capacity to assign to underlying lists.</param>
        public GuessResult(int wordIndex, int capacity)
        {
            GuessWordIndex = wordIndex;
            branches = new GuessBranch[Clues.ArrayLength];
            for(int clues = 0; clues < branches.Length; clues++)
                branches[clues] = new GuessBranch(clues, capacity);
        }

        /// <inheritdoc/>
        public int CompareTo(GuessResult other)
        {
            // The result with the least long branch goes first.
            var result = LargestCount.CompareTo(other.LargestCount);
            if (result != 0) return result;
            // A result with a correct answer goes before a result without a correct answer.
            return other.branches[Clues.Correct].WordIndices.Count.CompareTo(branches[Clues.Correct].WordIndices.Count);
        }

        /// <summary>
        /// Returns an enumerator iterating through all branches in this guess.
        /// </summary>
        /// <returns>An enumerator iterating through all branches in this guess.</returns>
        public IEnumerator<GuessBranch> GetEnumerator() => branches.AsEnumerable().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
