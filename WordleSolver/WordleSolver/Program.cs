using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using static WordleSolver.Constants;

namespace WordleSolver
{
    class Program
    {
        private static readonly object consoleLock = new();

        private const Difficulty Difficulty = WordleSolver.Difficulty.Normal;

        private static readonly Stopwatch stopwatch = new();

        private static long nextUpdated = 0;

        static void Main(string[] args)
        {
            string difficultyName = Difficulty.ToString().ToLower();
            stopwatch.Start();
            WriteLine($"Started solving for {difficultyName} mode at {DateTime.Now:HH:mm:ss zzz}");
            WriteLine("Initialising...");
            string[] words = File.ReadAllLines("wordle.txt");
            Solver solver = new(words, Difficulty);
            solver.ProgressChanged += ProgressChanged;
            Solution solution = solver.Solve();
            if (solution == null)
            {
                WriteLine($"No solution found using at most {MaximumGuesses} guesses.");
            }
            else
            {
                WriteLine("Solved!");
                string filename = $"wordle-{difficultyName}.json";
                WriteLine($"Saving {filename}");
                using (Stream file = File.OpenWrite(filename))
                using (Utf8JsonWriter writer = new(file, new() { Indented = true }))
                {
                    JsonSerializer.Serialize(writer, solution, new() { WriteIndented = true });
                }
                WriteLine("Done!");
            }
            stopwatch.Stop();
        }

        private static void ProgressChanged(int count, int total)
        {
            lock (consoleLock)
            {
                if (nextUpdated < stopwatch.ElapsedMilliseconds)
                {
                    nextUpdated = stopwatch.ElapsedMilliseconds + 1000;
                    WriteLine($"Solved {count} of {total} words.    ");
                }
            }
        }

        private static void WriteLine(string value)
        {
            lock (consoleLock)
            {
                Console.WriteLine($"{stopwatch.Elapsed:hh\\:mm\\:ss} {value}");
            }
        }
    }
}
