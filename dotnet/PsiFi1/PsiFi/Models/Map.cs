using PsiFi.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Models
{
    class Map
    {
        /// <summary>
        /// Gets the cell at the specified location.
        /// </summary>
        /// <param name="location">The cell's location.</param>
        /// <returns>The cell at the specified location.</returns>
        public Cell this[Point location] => Cells[location.X, location.Y];

        /// <summary>
        /// Gets the cell at the speicified location.
        /// </summary>
        /// <param name="x">The cell's X location.</param>
        /// <param name="y">The cell's Y location.</param>
        /// <returns>The cell at the specified location.</returns>
        public Cell this[int x, int y] => Cells[x, y];

        /// <summary>
        /// This map's height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// This map's width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The actors on this map.
        /// </summary>
        public ActorQueue Actors { get; } = new ActorQueue();

        /// <summary>
        /// This map's cells.
        /// </summary>
        public Cell[,] Cells { get; set; }

        /// <summary>
        /// Gets all cells on this map as an <see cref="IEnumerable{Cell}"/>.
        /// </summary>
        public IEnumerable<Cell> AllCells => Cells.Cast<Cell>();

        /// <summary>
        /// Initialises a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="width">This map's width.</param>
        /// <param name="height">This map's height.</param>
        public Map(int width, int height)
        {
            Width = width;
            Height = height;

            Cells = new Cell[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    Cells[x, y] = new Cell(x, y);
        }
    }
}
