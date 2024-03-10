using System.Collections;
using System.Collections.Generic;
namespace MarkRichardson.Battleship
{
    /// <summary>
    /// Represents the location of the top or left end of a ship and the direction it is facing.
    /// </summary>
    public struct Ship : IEnumerable<Cell>
    {

        /// <summary>
        /// The ship's length.
        /// </summary>
        public int Length;

        /// <summary>
        /// The ship's orientation.
        /// </summary>
        public Orientation Orientation;

        /// <summary>
        /// The X coordinate of the left of the ship.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y coordinate of the top of the ship.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initialises a new instance of the <see cref="Ship"/> struct.
        /// </summary>
        /// <param name="x">The ship's left-most coordinate.</param>
        /// <param name="y">The ship's top-most coordinate.</param>
        /// <param name="length">The ship's length.</param>
        /// <param name="orientation">The ship's orientation.</param>
        public Ship(int x, int y, int length, Orientation orientation)
        {
            X = x;
            Y = y;
            Length = length;
            Orientation = orientation;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Ship"/> struct.
        /// </summary>
        /// <param name="cell">The ship's top-left-most coordinates.</param>
        /// <param name="length">The ship's length.</param>
        /// <param name="orientation">The ship's orientation.</param>
        public Ship(Cell cell, int length, Orientation orientation)
        {
            X = cell.X;
            Y = cell.Y;
            Length = length;
            Orientation = orientation;
        }

        /// <summary>
        /// Gets all cells this ship is within.
        /// </summary>
        /// <returns>An enumeration of all cells this ship is within.</returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            var delta = Orientation == Orientation.Horizontal
                ? new Delta(1, 0)
                : new Delta(0, 1);

            var current = new Cell(X, Y);
            for (int i = 0; i < Length; i++)
            {
                yield return current;
                current += delta;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Transforms this object into a <see cref="Cell"/> object.
        /// </summary>
        /// <param name="obj">The object to transform.</param>
        /// <returns>The transformed object.</returns>
        public static explicit operator Cell(Ship obj)
        {
            return new Cell(obj.X, obj.Y);
        }
    }
}
