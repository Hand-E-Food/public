using MarkRichardson.Battleship.Gunners;
using MarkRichardson.Battleship.Navy;
using System;
using System.Threading;

namespace MarkRichardson.Battleship.Playback
{

    class Program
    {

        /// <summary>
        /// The time to wait between attacks in milliseconds.
        /// </summary>
        private const int SleepPeriod = 1000;

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var battlefield = new Battlefield();
            IGunnerFactory gunnerFactory = new Gunners.HeatMapperMk5.GunnerFactory();
            IGunner gunner = gunnerFactory.CreateGunner(battlefield);
            DrawInitialBattlefield();
            DrawGunnerName(gunnerFactory.Name);
            DrawAttacks(0);
            Battle(gunner);
            Console.WriteLine("Victory!");
            Console.ReadLine();
        }

        /// <summary>
        /// Draws the initial battlefield.
        /// </summary>
        private static void DrawInitialBattlefield()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            var row = new string('·', Battlefield.Size);
            for (int y = 0; y < Battlefield.Size; y++)
            {
                Console.WriteLine(row);
            }
        }

        /// <summary>
        /// Draws the gunner's name.
        /// </summary>
        /// <param name="name">The gunner's name.</param>
        private static void DrawGunnerName(string name)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Battlefield.Size + 1;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Draws the number of attacks made.
        /// </summary>
        /// <param name="attacks">The number of attacks made.</param>
        private static void DrawAttacks(int attacks)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorLeft = 0;
            Console.CursorTop = Battlefield.Size + 2;
            Console.WriteLine("Attacks: {0}", attacks);
        }

        /// <summary>
        /// Enacts the battle in its entirety.
        /// </summary>
        /// <param name="gunner">The gunner making the attacks.</param>
        private static void Battle(IGunner gunner)
        {
            BattlefieldStatus attack = null;
            do
            {
                DateTime trigger = DateTime.Now.AddMilliseconds(SleepPeriod);
                attack = gunner.Fire();
                Thread.Sleep(trigger - DateTime.Now);
                Console.CursorLeft = attack.Cell.X;
                Console.CursorTop  = attack.Cell.Y;
                switch (attack.AttackResult)
                {
                    case AttackResult.Miss:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write('~');
                        break;
                    case AttackResult.Hit:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('X');
                        break;
                    case AttackResult.Sunk:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('X');
                        break;
                }
                DrawAttacks(attack.Attacks);
            }
            while (!attack.Victory);
        }
    }
}
