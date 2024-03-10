using System;

namespace WordleSolver
{
    /// <summary>
    /// The state of a guess.
    /// </summary>
    public sealed class GuessResult : IComparable<GuessResult>
    {
        /// <summary>
        /// True if this guess could be a winning guess.
        /// False if this guess only narrows down the options.
        /// </summary>
        public bool IsAnswer;

        /// <summary>
        /// The index of the word that was guessed.
        /// </summary>
        public int GuessIndex;

        /// <summary>
        /// The number of target words in this guess's largest branch.
        /// </summary>
        public int LargestCount;

        /// <inheritdoc/>
        public int CompareTo(GuessResult other)
        {
            int result;
            
            // The result with the least longest branch goes first.
            result = this.LargestCount.CompareTo(other.LargestCount);
            if (result != 0) return result;

            // A result with a correct answer goes before a result without a correct answer.
            result = other.IsAnswer.CompareTo(this.IsAnswer);
            return result;
        }
    }
}
