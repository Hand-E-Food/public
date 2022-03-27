using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace WordleSolver
{
    class Program
    {
        private const bool HardMode = true;

        private static readonly TimeSpan ProgressReportThrottle = TimeSpan.FromSeconds(1);

        private static TimeSpan nextProgressReport = TimeSpan.Zero;
        private static readonly Stopwatch stopwatch = new();

        static void Main(string[] args)
        {
            stopwatch.Start();
            WriteLine($"Initialising...");
            var words = File.ReadAllLines("wordle.txt");
            var solver = new Solver(words, HardMode);
            solver.Progress += ReportProgress;
            var solution = solver.Solve();
            WriteLine($"Solved!");
            var filename = $"wordle-{(HardMode ? "hard" : "normal")}.json";
            WriteLine($"Saving {filename}");
            using (var file = File.OpenWrite(filename))
            using (var writer = new Utf8JsonWriter(file, new JsonWriterOptions { Indented = true }))
            {
                JsonSerializer.Serialize(writer, solution, new JsonSerializerOptions { WriteIndented = true });
            }
            WriteLine($"Done!");
            stopwatch.Stop();
        }

        private static void ReportProgress(int doneCount, int totalCount) 
        {
            if (nextProgressReport > stopwatch.Elapsed) return;
            nextProgressReport = stopwatch.Elapsed.Add(ProgressReportThrottle);
            WriteLine($"Solved {doneCount} of {totalCount} words.");
        }

        private static void WriteLine(string value) =>
            Console.WriteLine($"{stopwatch.Elapsed:hh:mm:ss}: {value}");
    }
}
