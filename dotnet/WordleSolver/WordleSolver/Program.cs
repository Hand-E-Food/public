using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace WordleSolver
{
    class Program
    {
        private const Difficulty Difficulty = WordleSolver.Difficulty.Normal;
        private const string wordListFilename = "wordle-NewYorkTimes.txt";

        private static readonly object consoleLock = new();
        private static string DifficultyName => Difficulty.ToString().ToLower();
        private static readonly object fileLock = new();
        private static int messageLength = 0;
        private static long nextUpdated = 0;
        private static readonly Stopwatch stopwatch = new();

        static void Main(string[] args)
        {
            stopwatch.Start();
            WriteLine($"Started solving for {DifficultyName} mode at {DateTime.Now:HH:mm:ss zzz}");
            CreateSolver().Solve();
            WriteLine("Done!");
            stopwatch.Stop();
        }

        private static Solver CreateSolver()
        {
            WriteLine("Loading " + wordListFilename);
            string[] words = File.ReadAllLines(wordListFilename);
            WriteLine("Initialising...");
            Solver solver = new(words, Difficulty);
            solver.ProgressChanged += ProgressChanged;
            solver.SolutionUpdated += SolutionUpdated;
            return solver;
        }

        private static void ProgressChanged(int batch, int total, int count)
        {
            lock (consoleLock)
            {
                if (nextUpdated < stopwatch.ElapsedMilliseconds)
                {
                    nextUpdated = stopwatch.ElapsedMilliseconds + 1000;
                    WriteLine($"Solved {count} of {total} words of batch {batch}.", true);
                }
            }
        }

        private static void SolutionUpdated(Solution solution)
        {
            lock (fileLock)
            {
                string filename = $"wordle-{DifficultyName}.json";
                WriteLine($"Solved in {solution.MaximumDepth} maximum guesses, {solution.Score / solution.TotalAnswers:F4} guesses on average. Saved to {filename}");
                using (Stream file = File.OpenWrite(filename))
                using (Utf8JsonWriter writer = new(file, new() { Indented = true }))
                {
                    JsonSerializer.Serialize(writer, solution, new() { WriteIndented = true });
                    file.SetLength(file.Position);
                }
            }
        }

        private static void WriteLine(string value, bool holdCursor = false)
        {
            lock (consoleLock)
            {
                string message = $"{stopwatch.Elapsed:hh\\:mm\\:ss} {value}";
                Console.WriteLine(message.PadRight(messageLength));
                if (holdCursor)
                {
                    messageLength = message.Length;
                    Console.CursorTop--;
                }
                else
                {
                    messageLength = 0;
                }
            }
        }
    }
}
