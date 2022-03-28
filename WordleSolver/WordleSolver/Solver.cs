using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static WordleSolver.Constants;

namespace WordleSolver
{
    public class Solver
    {
        /// <summary>
        /// A list of all indices allowed for <see cref="Clues"/>.
        /// </summary>
        private static readonly IEnumerable<int> allClues = Enumerable.Range(0, Clues.ArrayLength);

        /// <summary>
        /// A list of all indices in <see cref="allWords"/>.
        /// </summary>
        private readonly int[] allIndices;

        /// <summary>
        /// All words to solve for.
        /// </summary>
        private readonly string[] allWords;

        /// <summary>
        /// The cache of precalculated guesses.
        /// </summary>
        private readonly int[/*guessWord*/,/*clues*/][/*targetWord*/] cache;

        /// <summary>
        /// The difficulty to solve for.
        /// </summary>
        public Difficulty Difficulty { get; }

        /// <summary>
        /// Manages the progress events.
        /// </summary>
        private readonly Progress progress;

        /// <summary>
        /// Raised when this solver's progress changes.
        /// </summary>
        public event Progress.ChangedEventHandler ProgressChanged
        {
            add => progress.Changed += value;
            remove => progress.Changed -= value;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solver"/> class.
        /// </summary>
        /// <param name="words">Every possible word.</param>
        /// <param name="difficulty">The rules to solve for.</param>
        public Solver(string[] words, Difficulty difficulty)
        {
            int wordCount = words.Length;
            allIndices = Enumerable.Range(0, wordCount).ToArray();
            allWords = words;
            cache = new int[wordCount, Clues.ArrayLength][];
            Difficulty = difficulty;
            progress = new Progress(wordCount);
            for (int i = 0; i < wordCount; i++)
                for (int j = 0; j < Clues.ArrayLength; j++)
                    cache[i, j] = Array.Empty<int>();
        }

        /// <summary>
        /// Creates the solution graph.
        /// </summary>
        /// <returns>The solution graph. <see langword="null"/> if no solution can be found within
        /// <see cref="MaximumGuesses"/> guesses.</returns>
        public Solution Solve()
        {
            Initialise();

            progress.Count = 0; // To trigger Changed event.

            return GetResults(allIndices)
                .OrderBy(result => result)
                .Select(result => GetSolution(result, allIndices, allIndices, 1, default))
                .FirstOrDefault(solution => solution != null);
        }

        /// <summary>
        /// Prepares the cache.
        /// </summary>
        private void Initialise()
        {
            Parallel.For(0, allWords.Length, guessIndex =>
            {
                foreach (var group in allIndices.GroupBy(targetIndex => CalculateClues(allWords[guessIndex], allWords[targetIndex])))
                {
                    cache[guessIndex, group.Key] = group.ToArray();
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

            char[] guessChars = guessWord.ToCharArray();
            char[] targetChars = targetWord.ToCharArray();
            Clues clues = new();

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
                char guessChar = guessChars[i];
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

        /// <summary>
        /// Solves for the remaining target words.
        /// </summary>
        /// <param name="targetIndices">The indices of the remaining target words.</param>
        /// <param name="depth">The number of this guess, starting from 1.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The solution graph. <see langword="null"/> if no soultion can be found within
        /// <see cref="MaximumGuesses"/> guesses.</returns>
        private Solution GetSolution(IList<int> guessIndices, IList<int> targetIndices, int depth, CancellationToken cancellationToken)
        {
            if (targetIndices.Count == 1)
            {
                progress.Increment();
                return new(allWords[targetIndices[0]], depth, true);
            }
            else if (depth >= MaximumGuesses)
            {
                return null;
            }
            else
            {
                return GetResults(guessIndices, targetIndices)
                    .OrderBy(result => result)
                    .Select(result => GetSolution(result, guessIndices, targetIndices, depth, cancellationToken))
                    .FirstOrDefault(solution => solution != null);
            }
        }

        private IEnumerable<GuessResult> GetResults(IEnumerable<int> guessIndices)
        {
            return guessIndices
                .Select(guessIndex => new GuessResult
                {
                    GuessIndex = guessIndex,
                    IsAnswer = true,
                    LargestCount = allClues.Max(clues => GetTargetIndices(guessIndex, clues).Length),
                });
        }

        private IEnumerable<GuessResult> GetResults(IEnumerable<int> guessIndices, ICollection<int> targetIndices)
        {
            return guessIndices
                .Select(guessIndex => new GuessResult
                {
                    GuessIndex = guessIndex,
                    IsAnswer = targetIndices.Contains(guessIndex),
                    LargestCount = allClues.Max(clues => FilterTargetIndices(guessIndex, clues, targetIndices).Count()),
                });
        }

        private Solution GetSolution(GuessResult result, IList<int> guessIndices, IList<int> targetIndices, int depth, CancellationToken cancellationToken)
        {
            int guessIndex = result.GuessIndex;
            
            if (Difficulty == Difficulty.Normal)
                guessIndices = guessIndices.Where(wordIndex => wordIndex != guessIndex).ToArray();

            Solution solution = new(allWords[guessIndex], depth, result.IsAnswer);
            if (solution.IsAnswer) progress.Increment();
            try
            {
                depth++;
                CancellationTokenSource cancellation = new();
                cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellation.Token).Token;
                ParallelOptions parallelOptions = new() { CancellationToken = cancellationToken };
                Parallel.For(0, Clues.ArrayLength - 1, parallelOptions, clues =>
                {
                    IList<int> nextTargetIndices = FilterTargetIndices(guessIndex, clues, targetIndices).ToArray();
                    if (nextTargetIndices.Count == 0) return;
                    IList<int> nextGuessIndices = Difficulty switch
                    {
                        Difficulty.Hard => nextTargetIndices,
                        Difficulty.Normal => guessIndices,
                        _ => Array.Empty<int>(),
                    };

                    Solution branchSolution = GetSolution(nextGuessIndices, nextTargetIndices, depth, cancellationToken);
                    solution.Branches[clues] = branchSolution;
                    if (branchSolution == null) cancellation.Cancel();
                });
                return solution;
            }
            catch (Exception ex) when (AllExceptionsAre<OperationCanceledException>(ex))
            {
                progress.Add(-solution.Score);
                return null;
            }
        }

        private int[] GetTargetIndices(int guessIndex, int clues) =>
            cache[guessIndex, clues];

        private IEnumerable<int> FilterTargetIndices(int guessIndex, int clues, ICollection<int> targetIndices) =>
            cache[guessIndex, clues].Where(targetIndices.Contains);

        /// <summary>
        /// Checks a graph of <see cref="AggregateException"/> classes that all of the inner
        /// exceptions end in type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of exceptions to find.</typeparam>
        /// <param name="ex">The exception to check.</param>
        /// <returns>True if <paramref name="ex"/> is <typeparamref name="T"/>, or is an <see
        /// cref="AggregateException"/> whos inner exceptions all resolve true when passed to this
        /// function.</returns>
        private static bool AllExceptionsAre<T>(Exception ex) where T : Exception =>
            ex is T || (ex is AggregateException aex && aex.InnerExceptions.All(AllExceptionsAre<T>));
    }
}
