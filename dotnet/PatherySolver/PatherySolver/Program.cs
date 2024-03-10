using PatherySolver.Player;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PatherySolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var display = new Display();

            var map = new Map(30,
                    "++++++++++++++++++++",
                    "+>              + =+",
                    "+>   !            =+",
                    "+> +     A     +  =+",
                    "+>       1        =+",
                    "+> +  +           =+",
                    "+>                =+",
                    "+>                =+",
                    "+>        +   +   =+",
                    "+>        +       =+",
                    "+>                =+",
                    "+>         +      =+",
                    "+>      + +       =+",
                    "+>            +   =+",
                    "+>                =+",
                    "++++++++++++++++++++");

            var sampleTop = map.Height + 2;
            Console.WindowHeight = sampleTop * 2;

            var playerEngine = new PlayerEngine(map);
            playerEngine.OptimalSolutionChanged += (sender, e) => display.Write(playerEngine.OptimalSolution, 0);
            display.Write(playerEngine.OptimalSolution, 0);
            var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            var task = playerEngine.Optimize(cancellation.Token);
            while(task.Status < TaskStatus.RanToCompletion)
            {
                Thread.Sleep(1000);
                display.Write(playerEngine.Sample, sampleTop);
            }
        }
    }
}
