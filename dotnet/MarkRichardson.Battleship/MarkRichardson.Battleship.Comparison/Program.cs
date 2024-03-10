using MarkRichardson.Battleship.Gunners;
using MarkRichardson.Battleship.Navy;
using System;
using System.Linq;

namespace MarkRichardson.Battleship.Comparison
{
    class Program
    {
        static void Main(string[] args)
        {
            var gunnerRecords = new GunnerRecord[] {
                new GunnerRecord(new Gunners.ScannerMk1.GunnerFactory()),
                new GunnerRecord(new Gunners.ScannerMk2.GunnerFactory()),
                new GunnerRecord(new Gunners.HeatMapperMk1.GunnerFactory()),
                new GunnerRecord(new Gunners.HeatMapperMk4.GunnerFactory()),
                new GunnerRecord(new Gunners.HeatMapperMk5.GunnerFactory()),
            };

            Console.WriteLine("      battlefields");
            foreach (var record in gunnerRecords)
                Console.WriteLine("      {0}", record.Name);
            Console.WriteLine();
            Console.WriteLine("Press any key to stop.");

            int seed = 0;
            var update = DateTime.Now.AddSeconds(1);
            do
            {
                seed++;
                foreach (var record in gunnerRecords)
                {
                    var attacks = record.Battle(new Battlefield(seed));
                    record.Attacks += attacks;
                }
                if (DateTime.Now > update)
                {
                    update = DateTime.Now.AddSeconds(1);
                    Console.CursorTop = 0;
                    Console.WriteLine("{0,5:F0}", seed);
                    foreach (var record in gunnerRecords)
                        Console.WriteLine("{0,5:F2}", (double)record.Attacks / (double)seed);
                }
            }
            while (!Console.KeyAvailable);

            while (Console.KeyAvailable)
                Console.ReadKey(true);
            Console.WriteLine();
            Console.WriteLine("Press Enter to exit.  ");
            Console.ReadLine();
        }
    }
}
