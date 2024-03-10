using System;
using System.Collections;
using System.Collections.Generic;

namespace MarkRichardson.Battleship
{

    /// <summary>
    /// A battlefield grid.
    /// </summary>
    public class Grid<T> : ICloneable, IEnumerable<Cell>
    {

        /// <summary>
        /// The grid cells that contain part of a ship.
        /// </summary>
        private readonly T[,] _grid;

        /// <summary>
        /// The length of each edge of the grid.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Grid"/> class.
        /// </summary>
        /// <param name="size">The length of each edge of the grid.</param>
        public Grid(int size)
        {
            Size = size;
            _grid = new T[size, size];
        }

        /// <summary>
        /// Gets or sets the grid cell's status.
        /// </summary>
        /// <param name="cell">The coordinates of the grid cell.</param>
        /// <value>The contents of the grid cell.</value>
        public T this[Cell cell]
        {
            get { return _grid[cell.X, cell.Y]; }
            set { _grid[cell.X, cell.Y] = value; }
        }

        /// <summary>
        /// Gets or sets the grid cell's status.
        /// </summary>
        /// <param name="x">The X coordinates of the grid cell.</param>
        /// <param name="y">The Y coordinates of the grid cell.</param>
        /// <value>The contents of the grid cell.</value>
        public T this[int x, int y]
        {
            get { return _grid[x, y]; }
            set { _grid[x, y] = value; }
        }

        /// <summary>
        /// Returns the coordinates of every cell in this grid.
        /// </summary>
        /// <returns>An enumeration of the coordinates of every cell in this grid.</returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            for (int y = _grid.GetLowerBound(1); y <= _grid.GetUpperBound(1); y++)
                for (int x = _grid.GetLowerBound(0); x <= _grid.GetUpperBound(0); x++)
                    yield return new Cell(x, y);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a deep clone of this object.
        /// </summary>
        /// <returns>A deep clone of this object.</returns>
        public Grid<T> Clone()
        {
            var clone = new Grid<T>(Size);

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    T item = _grid[x, y];
                    if (typeof(T).IsAssignableFrom(typeof(ICloneable)))
                        item = (T)((ICloneable)item).Clone();
                    clone[x, y] = item;
                }
            }
            return clone;
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
