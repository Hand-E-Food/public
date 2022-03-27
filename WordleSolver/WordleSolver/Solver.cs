using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WordleSolver.Constants;

namespace WordleSolver
{
    public class Solver
    {
        private readonly string[] allWords;
        private readonly int[] allWordIndices;
        private readonly int[,] cache;
        private readonly object syncLock = new();

        private int solvedWords = -1;
        private int totalWords;

        /// <summary>
        /// True to solve for hard mode, where each guess must respect previous clues.
        /// False to solve for normal mode, where each guess can be any valid word.
        /// </summary>
        public bool HardMode { get; }

        /// <summary>
        /// Raised when the solution is updated.
        /// </summary>
        public event ProgressEventHandler Progress;
        /// <summary>
        /// The signature of the <see cref="Progress"/> event.
        /// </summary>
        /// <param name="doneCount">The number of words solved.</param>
        /// <param name="totalCount">The total number of words to solve.</param>
        public delegate void ProgressEventHandler(int doneCount, int totalCount);
        /// <summary>
        /// Calculates the number of words solved and raises the <see cref="Progreess"/> event.
        /// </summary>
        protected void OnProgress()
        {
            lock (syncLock)
            {
                solvedWords++;
                Progress?.Invoke(solvedWords, totalWords);
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solver"/> class.
        /// </summary>
        /// <param name="words">Every possible word.</param>
        /// <param name="hardMode">
        /// True to solve for hard mode, where each guess must respect previous clues.
        /// False to solve for normal mode, where each guess can be any valid word.
        /// </param>
        public Solver(IEnumerable<string> words, bool hardMode)
        {
            allWords = words.OrderBy(word => word).ToArray();
            totalWords = allWords.Length;
            allWordIndices = new int[totalWords];
            for (int i = 0; i < totalWords; i++)
                allWordIndices[i] = i;
            HardMode = hardMode;
            cache = new int[totalWords, totalWords];
        }

        /// <summary>
        /// Creates a graph of guesses that leads to every possible word.
        /// </summary>
        /// <returns>The solution graph.</returns>
        public Solution Solve()
        {
            Initialise();
            OnProgress();
            return GetSolution(allWordIndices, 1);
        }

        private void Initialise()
        {
            Parallel.For(0, allWords.Length, i =>
            {
                var guessWord = allWords[i];
                for (int j = 0; j < allWords.Length; j++)
                {
                    var targetWord = allWords[j];
                    cache[i,j] = CalculateClues(guessWord, targetWord);
                }
            });
        }

        /// <summary>
        /// Calculates the clues returned for a specified guess and target word.
        /// </summary>
        /// <param name="guessWord">The word being guessed.</param>
        /// <param name="targetWord">The secret target word.</param>
        /// <returns>The clues for the guess.</returns>
        public static Clues CalculateClues(string guessWord, string targetWord)
        {
            const char used = '\0';

            var guessChars = guessWord.ToCharArray();
            var targetChars = targetWord.ToCharArray();
            var clues = new Clues();
            
            for (int i = 0; i < WordLength; i++)
            {
                if (guessChars[i] == targetChars[i])
                {
                    clues[i] = Clue.Correct;
                    guessChars[i] = used;
                    targetChars[i] = used;
                }
            }
            
            for (int i = 0; i < WordLength; i++)
            {
                var guessChar = guessChars[i];
                if (guessChar == used) continue;
                for (int j = 0; j < WordLength; j++)
                {
                    if (guessChar == targetChars[j])
                    {
                        clues[i] = Clue.Misplaced;
                        guessChars[i] = used;
                        targetChars[j] = used;
                        break;
                    }
                }
            }

            return clues;
        }

        private Solution GetSolution(ICollection<int> targetWordIndices, int depth)
        {
            if (targetWordIndices.Count == 1)
            {
                OnProgress();
                return new Solution(allWords[targetWordIndices.Single()], depth);
            }

            var guessWordIndices = HardMode ? targetWordIndices : allWordIndices;

            var result = GetBestResult(guessWordIndices, targetWordIndices);

            var solution = new Solution(allWords[result.GuessWordIndex], depth);

            if (targetWordIndices.Contains(result.GuessWordIndex))
                OnProgress();

            depth++;
            result
                .Where(branch => branch.WordIndices.Count > 0 && branch.Clues != Clues.Correct)
                .AsParallel()
                .ForAll(branch => solution.Branches[branch.Clues] = GetSolution(branch.WordIndices, depth));

            return solution;
        }

        private GuessResult GetBestResult(IEnumerable<int> guessWordIndices, ICollection<int> targetWordIndices)
        {
            var largestCountThreshold = int.MaxValue;
            GuessResult bestResult = null;
            foreach (var guessWordIndex in guessWordIndices)
            {
                var result = GetResult(guessWordIndex, targetWordIndices, largestCountThreshold);
                if (result == null) continue;
                largestCountThreshold = result.LargestCount;
                bestResult = result;
                if (largestCountThreshold == 1) break;
            }
            return bestResult;
        }

        private GuessResult GetResult(int guessWordIndex, ICollection<int> targetWordIndices, int largestCountThreshold)
        {
            var result = new GuessResult(guessWordIndex, targetWordIndices.Count);

            foreach (var targetWordIndex in targetWordIndices)
            {
                var clues = cache[guessWordIndex, targetWordIndex];
                var wordIndices = result[clues].WordIndices;
                wordIndices.Add(targetWordIndex);
                if (wordIndices.Count >= largestCountThreshold) return null;
            }

            return result;
        }
    }
}
