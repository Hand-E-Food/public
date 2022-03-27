using System;
using System.Collections.Generic;
using System.Linq;
using static WordleSolver.Constants;

namespace WordleSolver
{
    public class Solver
    {
        private readonly string[] allWords;
        private readonly int[,] cache;
        private int solvedWords = -1;
        private int totalWords;

        /// <summary>
        /// True to solve for hard mode, where each guess must respect previous clues.
        /// False to solve for normal mode, where each guess can be any valid word.
        /// </summary>
        public bool HardMode { get; }

        public event ProgressEventHandler Progress;
        public delegate void ProgressEventHandler(int done, int total);
        public void OnProgress()
        {
            solvedWords++;
            Progress?.Invoke(solvedWords, totalWords);
        }

        public Solver(IEnumerable<string> words, bool hardMode)
        {
            allWords = words.OrderBy(word => word).ToArray();
            totalWords = allWords.Length;
            cache = new int[totalWords, totalWords];
        }

        public Solution Solve()
        {
            Initialise();
            OnProgress();
            return GetSolution(allWords);
        }

        private void Initialise()
        {
            string guessWord, targetWord;
            for (int i = 0; i < allWords.Length; i++)
            {
                guessWord = allWords[i];
                for (int j = 0; j < allWords.Length; j++)
                {
                    targetWord = allWords[j];
                    cache[i,j] = CalculateClues(guessWord, targetWord);
                }
            }
        }

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

        private Solution GetSolution(ICollection<string> targetWords)
        {
            if (targetWords.Count == 1)
                return new Solution(targetWords.Single());

            var guessWords = HardMode ? targetWords : allWords;

            var result = GetResults(guessWords, targetWords)
                .Aggregate((a, b) => a.CompareTo(b) <= 0 ? a : b);

            var solution = new Solution(result.Guess);

            if (targetWords.Contains(solution.Guess))
                OnProgress();

            foreach (var branch in result)
            {
                if (branch.Words.Count == 0) continue;
                if (branch.Clues == Clues.Correct) continue;
                solution.Branches[branch.Clues] = GetSolution(branch.Words);
            }
            return solution;
        }

        private IEnumerable<GuessResult> GetResults(IEnumerable<string> guessWords, ICollection<string> targetWords) =>
            guessWords.Select(word => GetResult(word, targetWords));

        private GuessResult GetResult(string guessWord, IEnumerable<string> targetWords)
        {
            var result = new GuessResult(guessWord);

            foreach (var targetWord in targetWords)
            {
                var clues = GetClues(guessWord, targetWord);
                result[clues].Words.Add(targetWord);
            }

            return result;
        }

        public int GetClues(string guessWord, string targetWord) =>
            cache[GetCacheIndex(guessWord), GetCacheIndex(targetWord)];

        private int GetCacheIndex(string word) => Array.BinarySearch(allWords, word);
    }
}
