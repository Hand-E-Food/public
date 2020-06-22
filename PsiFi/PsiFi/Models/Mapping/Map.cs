using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Geometry;
using System;
using System.Collections.Generic;

namespace PsiFi.Models
{
    class Map
    {

        /// <summary>
        /// The cell at the specified location.
        /// </summary>
        /// <param name="location">The location of the cell.</param>
        public Cell this[Location location]
        {
            get => Cells[location.X, location.Y];
            set => Cells[location.X, location.Y] = value;
        }

        /// <summary>
        /// This map's actors.
        /// </summary>
        public ActorQueue Actors { get; } = new ActorQueue(50);

        /// <summary>
        /// This map's cells.
        /// </summary>
        public Cell[,] Cells { get; }

        /// <summary>
        /// The conditions that can end this map. Each element returns a non-null value if its condition is met.
        /// </summary>
        public List<Func<MapEndReason>> EndConditions { get; } = new List<Func<MapEndReason>>();

        /// <summary>
        /// This map's size.
        /// </summary>
        public Size Size { get; }

        /// <summary>
        /// Initialises  a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="size">This map's size.</param>
        public Map(Size size)
        {
            Size = size;
            Cells = new Cell[size.Width, size.Height];
            for (int y = 0; y < size.Height; y++)
                for (int x = 0; x < size.Width; x++)
                    Cells[x, y] = new Cell(new Location(x, y));
        }
    }
}
