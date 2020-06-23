using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Engines.MapGenerators
{
    class OpenSquare : IMapGenerator
    {
        private readonly IRandom random;
        private readonly IEnumerable<Mob> mobs;

        public OpenSquare(IRandom random, IEnumerable<Mob> mobs)
        {
            this.random = random;
            this.mobs = mobs;
        }

        public Map CreateMap()
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

            map.Cells[size.Width / 2, size.Height / 2].Terrain = Terrain.Exit("Home");

            var emptyCells = map.Cells.Cast<Cell>().Where(cell => cell.Terrain.AllowsMovement && cell.Mob == null).ToList();
            foreach (var mob in mobs)
            {
                var cell = random.Next(emptyCells);
                emptyCells.Remove(cell);
                cell.Mob = mob;
                map.Actors.Add(mob);
            }

            return map;
        }
    }
}
