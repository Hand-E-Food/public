using System;

namespace PatherySolver
{
    public struct Location : IComparable<Location>, IEquatable<Location>
    {
        public static readonly Location Empty = new();

        public readonly int X;
        public readonly int Y;

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <inheritdoc/>
        public int CompareTo(Location other)
        {
            int result = other.Y - Y;
            if (result != 0) return result;
            result = other.X - X;
            return result;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Location other && this == other;

        /// <inheritdoc/>
        public bool Equals(Location other) => this == other;

        /// <inheritdoc/>
        public override int GetHashCode() => (X << 16 | X >> 16) ^ Y;

        /// <inheritdoc/>
        public override string ToString() => $"{X},{Y}";

        public static bool operator ==(Location a, Location b) => a.X == b.X && a.Y == b.Y;
        
        public static bool operator !=(Location a, Location b) => !(a == b);

        public static Location operator +(Location a, Direction b) => new(a.X + b.DX, a.Y + b.DY);

        public static Location operator -(Location a, Direction b) => new(a.X - b.DX, a.Y - b.DY);

        public static Direction operator -(Location a, Location b) => new(a.X - b.X, a.Y - b.Y);
    }
}
