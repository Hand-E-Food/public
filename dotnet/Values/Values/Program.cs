// See https://aka.ms/new-console-template for more information
using Values;
using Values.Generation;

TextParser textParser = new();
CardDrawer cardDrawer = new(textParser);
CardCreator cardCreator = new(cardDrawer);
CardRepository cardRepository = new();
Engine engine = new(cardCreator, cardRepository);
engine.Run();
