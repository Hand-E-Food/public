using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Mapping
{
    /// <summary>
    /// A simple rectangular map.
    /// </summary>
    /// <inheritdoc/>
    public class Map<TCell> : IMap<TCell> where TCell : ICell
    {
        /// <summary>
        /// The cells on this map.
        /// </summary>
        protected TCell[,] cells = null!;

        /// <inheritdoc/>
        public TCell this[Point location]
        {
            get => cells[location.X, location.Y];
            set
            {
                value.Location = location;
                cells[location.X, location.Y] = value;
            }
        }

        /// <inheritdoc/>
        public TCell this[int x, int y]
        {
            get => cells[x, y];
            set
            {
                value.Location = new Point(x, y);
                cells[x, y] = value;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TCell> AllCells => cells.Cast<TCell>();

        /// <inheritdoc/>
        public IEnumerable<Point> AllLocations => Bounds.AllPoints;

        /// <summary>
        /// Gets this map's bounds.
        /// </summary>
        public Rectangle Bounds => new Rectangle(Size);

        /// <inheritdoc/>
        public Size Size { get; private set; } = Size.Empty;

        /// <inheritdoc/>
        public TCell GetCell(Point location) => cells[location.X, location.Y];

        /// <inheritdoc/>
        public TCell GetCell(int x, int y) => cells[x, y];

        /// <inheritdoc/>
        public virtual void Initialize(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), $"{nameof(size)} must have a positive {nameof(Size.Width)} and {nameof(Size.Height)}");

            Size = size;
            cells = new TCell[size.Width, size.Height];
        }
    }
}
