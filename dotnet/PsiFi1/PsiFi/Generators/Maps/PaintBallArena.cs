using PsiFi.Models;
using PsiFi.Models.Floors;
using PsiFi.Models.Mobs;
using PsiFi.Models.Structures;
using System.Linq;
using static PsiFi.Geometry.Math;
using static System.Math;

namespace PsiFi.Generators.Maps
{
    class PaintBallArena : EmptyMap
    {
        protected override void GenerateMap()
        {
            for (int wall = 0; wall < 20; wall++)
                AddRandomWall();

            Populate();
        }

        protected void AddRandomWall()
        {
            int x1 = Random.Next(2, Width - 2);
            int y1 = Random.Next(2, Height - 2);
            int x2 = x1, y2 = y1, dx, dy;

            if (State.Random.Next(2) == 0)
                x2 = Random.Next(Max(2, x1 - 7), Min(Width - 2, x1 + 7));
            else
                y2 = Random.Next(Max(2, y1 - 7), Min(Width - 2, y1 + 7));

            dx = Sign(x2 - x1);
            dy = Sign(y2 - y1);

            var structure = State.Random.NextDouble() >= 0.1
                ? (Structure)Wall.Indestructable
                : (Structure)Window.Indestructable;

            Map[x1, y1].Occupant = Wall.Indestructable;
            while (true)
            {
                x1 += dx;
                y1 += dy;
                if (x1 == x2 && y1 == y2) break;
                Map[x1, y1].Occupant = structure;
            }
            Map[x1, y1].Occupant = Wall.Indestructable;
        }

        protected void Populate()
        {
            const int PersonalSpace = 20;

            var emptyCells = Map.AllCells
                .Where(cell => cell.Occupant == null)
                .ToList();

            var cell = Random.Next(emptyCells);
            State.Player.Cell = cell;
            var playerLocation = cell.Location;
            emptyCells.RemoveAll(cell => Distance(playerLocation, cell.Location) <= PersonalSpace);

            for (int i = 0; i < 10; i++)
            {
                cell = Random.Next(emptyCells);
                emptyCells.Remove(cell);
                cell.Occupant = new Rat();
            }

            cell = Random.Next(emptyCells);
            emptyCells.Remove(cell);
            cell.Floor = new LevelExit();
        }
    }
}
