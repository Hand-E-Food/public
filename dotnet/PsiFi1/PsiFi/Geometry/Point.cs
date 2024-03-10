namespace PsiFi.Geometry
{
    struct Point
    {
        /// <summary>
        /// This point's X coordinate.
        /// </summary>
        public int X;

        /// <summary>
        /// This point's Y coordinate.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initialises a new instance of the <see cref="Point"/> structure.
        /// </summary>
        /// <param name="x">This point's X coordinate.</param>
        /// <param name="y">This point's Y coordinate.</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null
                && typeof(Point) == obj.GetType()
                && this == (Point)obj;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => unchecked((Y + int.MinValue) ^ X);

        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(Point left, Point right) => !(left == right);

        public static Point operator +(Point left, Vector right) => new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator -(Point left, Vector right) => new Point(left.X - right.X, left.Y - right.Y);

        public static Vector operator -(Point left, Point right) => new Vector(left.X - right.X, left.Y - right.Y);
    }
}
