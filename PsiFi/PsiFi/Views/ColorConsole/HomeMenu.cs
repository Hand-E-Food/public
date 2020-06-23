using PsiFi.Models;
using System;

namespace PsiFi.Views.ColorConsole
{
    class HomeMenu : IUserInterface<HomeMenuResponse>
    {
        private readonly Campaign campaign;

        public HomeMenu(Campaign campaign)
        {
            this.campaign = campaign;
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
            int max = Math.Min(9, campaign.MissionOffers.Count);
            for (int i = 0; i < max; i++)
            {
                var mission = campaign.MissionOffers[i];
                keyActions.Add(ConsoleKey.D1 + i, () => HomeMenuResponse.StartMission(mission));
                Console.WriteLine($"{(char)('1' + i)}. {mission.Name}");
            }
            Console.WriteLine();
            Console.WriteLine("System");
            Console.WriteLine("------");
            Console.WriteLine("q. Quit");
            keyActions.Add(ConsoleKey.Q, () => HomeMenuResponse.Quit);
            return keyActions.ReadKey();
        }
    }
}
