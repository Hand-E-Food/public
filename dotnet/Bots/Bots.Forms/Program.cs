using Bots.Engine;
using Bots.Forms;
using Bots.Models;

ApplicationConfiguration.Initialize();

Map map = new(new(21, 21));

Bot bot = new() {
    Character = '@',
    ForeColor = Color.Black,
    Map = map,
    Pose = new(new(10, 10), Vector.North),
};

GameMaster gameMaster = new() {
    Map = map,
};

MapForm mainForm = new() {
    GameMaster = gameMaster,
    Map = map,
};

Application.Run(mainForm);
