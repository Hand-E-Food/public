using System;

namespace PatherySolver
{
    public struct Direction : IEquatable<Direction>
    {
        public static readonly Direction None = new(0, 0);
        public static readonly Direction Down = new(0, +1);
        public static readonly Direction Left = new(-1, 0);
        public static readonly Direction Right = new(+1, 0);
        public static readonly Direction Up = new(0, -1);
        public static readonly Direction[] All = new[] { Up, Right, Down, Left };

        public readonly int DX;
        public readonly int DY;

        public Direction(int dx, int dy)
        {
            DX = dx;
            DY = dy;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Direction other && this == other;

        /// <inheritdoc/>
        public bool Equals(Direction other) => this == other;

        /// <inheritdoc/>
        public override int GetHashCode() => (DX << 16 | DX >> 16) ^ DY;

        /// <inheritdoc/>
        public override string ToString() => $"{DX:+0;-0},{DY:+0;-0}";

        public static bool operator ==(Direction a, Direction b) => a.DX == b.DX && a.DY == b.DY;

        public static bool operator !=(Direction a, Direction b) => !(a == b);

        public static Direction operator +(Direction a, Direction b) => new(a.DX + b.DX, a.DY + b.DY);

        public static Direction operator -(Direction a, Direction b) => new(a.DX - b.DX, a.DY - b.DY);

        public static Direction operator *(Direction a, int multiplier) => new(a.DX * multiplier, a.DY * multiplier);
    }
}
