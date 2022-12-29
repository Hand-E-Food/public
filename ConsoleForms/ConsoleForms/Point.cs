using System.Diagnostics.CodeAnalysis;

namespace ConsoleForms
{
    /// <summary>
    /// A 2-dimensional point.
    /// </summary>
    public struct Point : IEquatable<Point>
    {
        public override string ToString() => $"{X},{Y}";

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

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point other && Equals(other);
        public bool Equals(Point other) => this == other;
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => !(a == b);

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
