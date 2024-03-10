using System;
using System.Threading;

namespace DragonMania.MapPlacement
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancellation = new CancellationTokenSource();
            var data = new DataLoader();
            var buildings = data.GetBuildings();
            var map = data.GetMap();
            var view = new ConsoleView(map);
            var engine = new OptimizationEngine(map, buildings, cancellation.Token);
            engine.SolutionUpdated += view.DisplaySolution;

            var thread = new Thread(engine.Optimize);
            thread.Start();
            WaitForExit();
            cancellation.Cancel();
            thread.Join();
        }

        private static void WaitForExit()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
