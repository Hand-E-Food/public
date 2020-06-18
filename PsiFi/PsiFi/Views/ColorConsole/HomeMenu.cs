using PsiFi.Models;
using System;

namespace PsiFi.Views.ColorConsole
{
    class HomeMenu : IUserInterface<HomeMenuResponse>
    {
        private readonly World world;

        public HomeMenu(World world)
        {
            this.world = world;
        }

        public HomeMenuResponse GetInput()
        {
            var keyActions = new KeyActionCollection<HomeMenuResponse>();

            Console.Clear();
            Console.WriteLine("Home");
            Console.WriteLine("====");
            Console.WriteLine();
            Console.WriteLine("Missions");
            Console.WriteLine("--------");
            Console.WriteLine();
            for (int i = 0; i < world.MissionOffers.Count; i++)
            {
                var mission = world.MissionOffers[i];
                var key = new ConsoleKeyInfo((char)('0' + i), ConsoleKey.D0 + i, false, false, false);
                Func<HomeMenuResponse> action = () => HomeMenuResponse.StartMission(mission);
                keyActions.Add(key, action);
                Console.WriteLine($"{key.KeyChar}. {mission.Name}");
            }
            Console.WriteLine();
            Console.WriteLine("System");
            Console.WriteLine("------");
            Console.WriteLine("q. Quit");
            keyActions.Add(new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false), () => HomeMenuResponse.Quit);
            return keyActions.ReadKey();
        }
    }
}
