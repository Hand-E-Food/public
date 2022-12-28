// See https://aka.ms/new-console-template for more information

using PsiFi;
using PsiFi.ConsoleView;

Protagonist protagonist = new();
GameEngine gameEngine = new(protagonist);
GameView gameView = new(gameEngine);
gameView.Run();