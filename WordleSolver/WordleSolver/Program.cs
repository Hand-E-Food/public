using System;
using System.IO;
using System.Text.Json;

namespace WordleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = File.ReadAllLines("wordle.txt");
            Console.WriteLine($"Initialising...");
            var solver = new Solver(words, false);
            solver.Progress += ReportProgress;
            var solution = solver.Solve();
            using var file = File.OpenWrite("wordle.json");
            using var writer = new Utf8JsonWriter(file);
            JsonSerializer.Serialize(writer, solution);
        }

        private static void ReportProgress(int done, int total)
        {
            Console.CursorTop--;
            Console.WriteLine($"Solved {done} of {total} words.");
        }
    }
}
