using ColourBlind;

TemplateFactory templateFactory = new();
Template template = templateFactory.CreateTemplate();
string outDir = Path.Join("..", "..", "..", "..", "assets");
CardFactory cardFactory = new(template, outDir);
cardFactory.CreateCards();
