// See https://aka.ms/new-console-template for more information

using ConsoleForms;
using PsiFi;
using PsiFi.ConsoleView;

Bitmap bitmap = new(80, 30);
Canvas canvas = new(bitmap);
Protagonist protagonist = new();
Game game = new(protagonist);
GameEngine gameEngine = new(game);
HomeView homeView = new(game);
homeView.Run(canvas, gameEngine.RunGame());
