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
        public GuessBranch this[Clues clues] => branches[clues];

        /// <summary>
        /// The word that was guessed.
        /// </summary>
        public string Guess { get; }

        /// <summary>
        /// True if this guess is guaranteed to be correct.
        /// </summary>
        public bool IsDefinitelyCorrect => branches[Clues.Correct].Words.Count == 1 && branches.Sum(branch => branch.Words.Count) == 1;

        /// <summary>
        /// The count of the largest branch.
        /// </summary>
        public int LargestCount => branches.Max(branch => branch.Words.Count);

        /// <summary>
        /// Creates an object to store the results of a guess.
        /// </summary>
        /// <param name="word">The word to guess.</param>
        public GuessResult(string word)
        {
            Guess = word;
            branches = new GuessBranch[Clues.ArrayLength];
            for(int clues = 0; clues < branches.Length; clues++)
                branches[clues] = new GuessBranch(clues);
        }

        /// <inheritdoc/>
        public int CompareTo(GuessResult other)
        {
            // The result with the least long branch goes first.
            var result = LargestCount.CompareTo(other.LargestCount);
            if (result != 0) return result;
            // A result with a correct answer goes before a result without a correct answer.
            return other.branches[Clues.Correct].Words.Count.CompareTo(branches[Clues.Correct].Words.Count);
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
