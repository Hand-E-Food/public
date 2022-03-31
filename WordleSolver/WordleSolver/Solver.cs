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
        /// The maximum number of gueses that can be used.
        /// </summary>
        private int maximumDepth = 6;

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
        /// Raised when a better solution is found.
        /// </summary>
        public event SolutionUpdatedEventHandler SolutionUpdated;

        private void OnSolutionUpdated(Solution solution) => SolutionUpdated?.Invoke(solution);

        /// <summary>
        /// The signature of the <see cref="SolutionUpdated"/> event.
        /// </summary>
        /// <param name="solution">The new solution.</param>
        public delegate void SolutionUpdatedEventHandler(Solution solution);

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
            progress.NextBatch();
            Solution solution = OrderByQuality(allIndices)
                .Select(guessIndex => GetSolution(guessIndex, allIndices, allIndices, 1, default))
                .FirstOrDefault(solution => solution != null);

            if (solution != null) OnSolutionUpdated(solution);
            return solution;
        }

        /// <summary>
        /// Prepares the cache.
        /// </summary>
        private void Initialise()
        {
            Parallel.For(0, allWords.Length, guessIndex =>
            {
                foreach (var group in allIndices.GroupBy(targetIndex => new Clues(allWords[guessIndex], allWords[targetIndex]).GetHashCode()))
                {
                    cache[guessIndex, group.Key] = group.ToArray();
                }
            });
        }

        /// <summary>
        /// Solves for the remaining target words.
        /// </summary>
        /// <param name="targetIndices">The indices of the remaining target words.</param>
        /// <param name="depth">The number of this guess, starting from 1.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The solution graph. <see langword="null"/> if no soultion can be found within
        /// <see cref="MaximumGuesses"/> guesses.</returns>
        private Solution GetSolution(int[] guessIndices, int[] targetIndices, int depth, CancellationToken cancellationToken)
        {
            if (targetIndices.Length == 1)
            {
                progress.Increment();
                return new(allWords[targetIndices[0]], depth, true);
            }
            else if (depth >= maximumDepth)
            {
                return null;
            }
            else
            {
                return OrderByQuality(guessIndices, targetIndices)
                    .Select(guessIndex => GetSolution(guessIndex, guessIndices, targetIndices, depth, cancellationToken))
                    .FirstOrDefault(solution => solution != null);
            }
        }

        private int[] OrderByQuality(int[] guessIndices)
        {
            return guessIndices
                .Select(guessIndex => new GuessResult
                {
                    GuessIndex = guessIndex,
                    IsAnswer = true,
                    LargestCount = allClues.Max(clues => cache[guessIndex, clues].Length),
                })
                .OrderBy(result => result)
                .Select(result => result.GuessIndex)
                .ToArray();
        }

        private int[] OrderByQuality(int[] guessIndices, int[] targetIndices)
        {
            return guessIndices
                .Select(guessIndex => new GuessResult
                {
                    GuessIndex = guessIndex,
                    IsAnswer = targetIndices.Contains(guessIndex),
                    LargestCount = allClues.Max(clues => FilterTargetIndices(guessIndex, clues, targetIndices).Count()),
                })
                .OrderBy(result => result)
                .Select(result => result.GuessIndex)
                .ToArray();
        }

        private Solution GetSolution(int guessIndex, int[] guessIndices, int[] targetIndices, int depth, CancellationToken cancellationToken)
        {
            int[] nextGuessIndices = null;
            if (Difficulty == Difficulty.Normal)
            {
                nextGuessIndices = new int[guessIndices.Length - 1];
                int i = Array.IndexOf(guessIndices, guessIndex);
                Array.Copy(guessIndices, 0, nextGuessIndices, 0, i);
                Array.Copy(guessIndices, i + 1, nextGuessIndices, i, nextGuessIndices.Length - i);
            }

            Solution solution = new(allWords[guessIndex], depth, targetIndices.Contains(guessIndex));
            if (solution.IsAnswer) progress.Increment();
            try
            {
                depth++;
                CancellationTokenSource cancellation = new();
                cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cancellation.Token).Token;
                ParallelOptions parallelOptions = new() { CancellationToken = cancellationToken };
                Parallel.For(0, Clues.ArrayLength - 1, parallelOptions, clues =>
                {
                    int[] nextTargetIndices = FilterTargetIndices(guessIndex, clues, targetIndices).ToArray();
                    if (nextTargetIndices.Length == 0) return;

                    if (Difficulty == Difficulty.Hard)
                        nextGuessIndices = nextTargetIndices;

                    Solution branchSolution = GetSolution(nextGuessIndices, nextTargetIndices, depth, cancellationToken);
                    solution.Branches[clues] = branchSolution;
                    if (branchSolution == null) cancellation.Cancel();
                });
                return solution;
            }
            catch (Exception ex) when (AllExceptionsAre<OperationCanceledException>(ex))
            {
                progress.Add(-solution.TotalAnswers);
                return null;
            }
        }

        private IEnumerable<int> FilterTargetIndices(int guessIndex, int clues, int[] targetIndices)
        {
            int[] allTargetIndices = cache[guessIndex, clues];
            for (int i = 0, j = 0; i < allTargetIndices.Length && j < targetIndices.Length;)
            {
                int targetIndex = allTargetIndices[i];
                int comparison = targetIndex.CompareTo(targetIndices[j]);
                if (comparison < 0)
                {
                    i++;
                }
                else if (comparison > 0)
                {
                    j++;
                }
                else
                {
                    yield return targetIndex;
                    i++;
                    j++;
                }
            }
        }

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
