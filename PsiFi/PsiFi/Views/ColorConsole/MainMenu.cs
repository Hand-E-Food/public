﻿using System;

namespace PsiFi.Views.ColorConsole
{
    class MainMenu : IUserInterface<MainMenuResponse>
    {
        private readonly KeyActionCollection<MainMenuResponse> keyActions;

        public MainMenu()
        {
            keyActions = new KeyActionCollection<MainMenuResponse> {
                { ConsoleKey.N, () => MainMenuResponse.NewGame },
                { ConsoleKey.Q, () => MainMenuResponse.Quit },
            };
            Console.CursorVisible = false;
            Console.SetWindowSize(80, 50);
            Console.SetBufferSize(80, 50);
            Console.TreatControlCAsInput = true;
            Console.Title = "Psi-Fi";
        }

        public MainMenuResponse GetInput()
        {
            Console.Clear();
            Console.WriteLine("Psi-Fi");
            Console.WriteLine("======");
            Console.WriteLine();
            Console.WriteLine("System");
            Console.WriteLine("------");
            Console.WriteLine();
            Console.WriteLine("n. New game");
            Console.WriteLine("q. Quit");
            return keyActions.ReadKey();
        }
    }
}