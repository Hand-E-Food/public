using Rogue;
using Rogue.Mapping.Creation;
using System;

namespace PsiFi.Mapping.Creation
{
    public class CellFactory : ICellFactory<Cell>
    {
        private readonly Random random = new Random();

        public Cell CreateSpace()
        {
            return new Cell {
                BackColor = Color.Black,
                Character = '.',
                ForeColor = Color.Gray,
                IsTransparent = true,
                IsWalkable = true,
            };
        }

        public Cell CreateWall()
        {
            return new Cell {
                BackColor = Color.Black,
                Character = '#',
                ForeColor = new Color((byte)random.Next(128,256), (byte)random.Next(128, 256), (byte)random.Next(128, 256)),
                IsTransparent = false,
                IsWalkable = false,
            };
        }
    }
}
