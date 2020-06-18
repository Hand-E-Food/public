using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Actors.Mobs;

namespace PsiFi.Engines.MapGenerators
{
    class OpenSquare : IMapGenerator
    {
        public Map CreateMap(Player player)
        {
            var size = new Size(80, 40);
            var map = new Map(size);

            int x, y;
            for (y = 0; y < size.Height; y++)
                for (x = 0; x < size.Width; x++)
                    map.Cells[x, y].Terrain = Terrain.Floor;

            y = size.Height - 1;
            for (x = 0; x < size.Width; x++)
            {
                map.Cells[x, 0].Terrain = Terrain.Wall;
                map.Cells[x, y].Terrain = Terrain.Wall;
            }

            x = size.Width - 1;
            for (y = 0; y < size.Height; y++)
            {
                map.Cells[0, y].Terrain = Terrain.Wall;
                map.Cells[x, y].Terrain = Terrain.Wall;
            }

            map.Cells[3, 3].Mob = player;
            map.Cells[size.Width - 4, size.Height - 4].Terrain = Terrain.Exit("Home");

            return map;
        }
    }
}
