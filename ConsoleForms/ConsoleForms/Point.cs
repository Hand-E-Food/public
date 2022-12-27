namespace ConsoleForms
{
    /// <summary>
    /// A 2-dimensional point.
    /// </summary>
    public struct Point
    {
        /// <summary>
        /// Creates a new <see cref="Point"/>.
        /// </summary>
        /// <param name="x">This point's x coordinate.</param>
        /// <param name="y">This point's y coordinate.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// This point's x coordinate.
        /// </summary>
        public int X;

        /// <summary>
        /// This point's y coordinate.
        /// </summary>
        public int Y;
    }
}
