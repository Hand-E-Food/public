namespace MarkRichardson.Battleship
{

    /// <summary>
    /// The coordinates of a grid cell.
    /// </summary>
    public struct Cell
    {

        /// <summary>
        /// The X coordinate.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y coordinate.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initialises a new instance of the <see cref="Cell"/> structure.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns whether or not this cell coordinate is within the specified bounds.
        /// </summary>
        /// <param name="size">The maximum exclusive value for X and Y.</param>
        /// <returns>True if X and Y are both greater than or equal to 0 and less than size; otherwise, false.</returns>
        public bool IsValid(int size)
        {
            return X >= 0
                && X < size
                && Y >= 0
                && Y < size;
        }

        /// <summary>
        /// Returns the cell offset by the specified delta.
        /// </summary>
        /// <param name="cell">The origin cell.</param>
        /// <param name="delta">The offset.</param>
        /// <returns>The cell offset by the specified delta.</returns>
        public static Cell operator + (Cell cell, Delta delta)
        {
            return new Cell(cell.X + delta.DX, cell.Y + delta.DY);
        }
    }
}
