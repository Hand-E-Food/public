using System.Collections.Generic;

namespace Rogue.Mapping
{
    /// <summary>
    /// A map of cells.
    /// </summary>
    /// <typeparam name="TCell">The type of cells in this map.</typeparam>
    public interface IMap<TCell> where TCell : ICell
    {
        /// <summary>
        /// The cell at the specified location.
        /// </summary>
        /// <param name="point">The cell's location.</param>
        TCell this[Point location] { get; set; }

        /// <summary>
        /// The cell at the specified location.
        /// </summary>
        /// <param name="x">The cell's X coordinate.</param>
        /// <param name="y">The cell's Y coordinate.</param>
        TCell this[int x, int y] { get; set; }

        /// <summary>
        /// All cells on this map.
        /// </summary>
        IEnumerable<TCell> AllCells { get; }

        /// <summary>
        /// Every location within this map.
        /// </summary>
        IEnumerable<Point> AllLocations { get; }

        /// <summary>
        /// This map's size in cells.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Gets the cell at the specified location.
        /// </summary>
        /// <param name="point">The cell's location.</param>
        /// <returns>The cell at the specified location.</returns>
        TCell GetCell(Point location);

        /// <summary>
        /// Gets the cell at the specified location.
        /// </summary>
        /// <param name="x">The cell's X coordinate.</param>
        /// <param name="y">The cell's Y coordinate.</param>
        /// <returns>The cell at the specified location.</returns>
        TCell GetCell(int x, int y);

        /// <summary>
        /// Initialises this map.
        /// </summary>
        /// <param name="size">This map's size in cells.</param>
        void Initialize(Size size);
    }
}
