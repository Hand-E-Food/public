using static System.Math;

namespace ColourBlind
{
    /// <summary>
    /// A line between two 2-dimensional points.
    /// </summary>
    public struct LineD
    {
        /// <summary>
        /// Gradient constant for y = a + bx
        /// </summary>
        private readonly double a;

        /// <summary>
        /// Gradient constant for y = a + bx
        /// </summary>
        private readonly double b;

        /// <summary>
        /// This line's left-most coordinate.
        /// </summary>
        private readonly double left;

        /// <summary>
        /// This line's right-most coordinate.
        /// </summary>
        private readonly double right;

        /// <summary>
        /// This line's starting point.
        /// </summary>
        public readonly PointD Start;

        /// <summary>
        /// This line's ending point.
        /// </summary>
        public readonly PointD End;

        /// <summary>
        /// Initialises a new <see cref="LineD"/>.
        /// </summary>
        /// <param name="start">This line's starting point.</param>
        /// <param name="end">This line's ending point.</param>
        public LineD(PointD start, PointD end)
        {
            Start = start;
            End = end;

            left = Min(start.X, end.X);
            right = Max(start.X, end.X);

            if (start.X != end.X)
            {
                b = (end.Y - start.Y) / (end.X - start.X);
                a = start.Y - b * start.X;
            }
            else
            {
                a = 0;
                b = 0;
            }
        }

        /// <summary>
        /// Tests if this line is directly below the specified point.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>True if this line is directly below the <paramref name="point"/>. Otherwise, false.</returns>
        public bool IsBelow(PointD point)
        {
            return point.X >= left
                && point.X <= right
                && point.X != End.X
                && point.Y <= a + point.X * b;
        }
    }
}
