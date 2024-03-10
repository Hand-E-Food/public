using static System.Math;

namespace OrixoSolver
{
    public struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) => obj is Point that && this == that;

        public Direction GetDirectionTo(Point other) => new Direction(Sign(other.X - X), Sign(other.Y - Y));

        public override int GetHashCode() => ((X << 16) | (X >> 16)) ^ Y;

        public static Point operator +(Point left, Direction right) => new Point(left.X + right.DX, left.Y + right.DY);

        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;

        public static bool operator !=(Point left, Point right) => !(left == right);
    }
}
