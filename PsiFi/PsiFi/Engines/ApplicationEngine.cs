﻿using PsiFi.Models;
using PsiFi.Views;
using PsiFi.Views.ColorConsole;

namespace PsiFi.Engines
{
    class ApplicationEngine
    {
        private readonly IUserInterface<MainMenuResponse> mainMenu;

        public ApplicationEngine(IUserInterface<MainMenuResponse> mainMenu)
        {
            this.mainMenu = mainMenu;
        }

        public void Begin()
        {
            var isRunning = true;
            while (isRunning)
            {
                var response = mainMenu.GetInput();
                switch (response.Action)
                {
                    case MainMenuAction.NewGame:
                        NewGame();
                        break;

                    case MainMenuAction.Quit:
                        isRunning = false;
                        break;
                }
            }
        }

        private void NewGame()
        {
            var campaign = new Campaign();
            var homeMenu = new HomeMenu(campaign);
            var gameEngine = new HomeEngine(campaign, homeMenu);
            gameEngine.Begin();
        }
    }
}