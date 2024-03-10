using System;
using System.Linq;

namespace DragonMania.MapPlacement
{
    public class ConsoleView
    {
        public ConsoleView(Map map)
        {
            Console.Title = "Dragon Mania Legends Map Optimization";
            var text = map.ToLines().ToList();
            var width = Math.Max(Console.WindowWidth, text.Max(x => x.Length));
            var height = Math.Max(Console.WindowHeight, text.Count + 11);
            Console.SetBufferSize(width, height);
        }

        public void DisplaySolution(Solution solution)
        {
            Console.Clear();
            Console.Write(solution.Map.ToString());
            Console.WriteLine();
            Console.WriteLine($"Remaining buildings (total size {solution.Score})");
            foreach (var building in solution.RemainingBuildings)
                Console.WriteLine($"{building.Size}x{building.Size}: {building.Count}");
        }
    }
}
