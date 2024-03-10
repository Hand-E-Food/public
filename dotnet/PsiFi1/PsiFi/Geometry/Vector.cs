using static System.Math;

namespace PsiFi.Geometry
{
    struct Vector
    {
        public static readonly Vector NW = new Vector(-1, -1);
        public static readonly Vector N  = new Vector( 0, -1);
        public static readonly Vector NE = new Vector(+1, -1);
        public static readonly Vector E  = new Vector(+1,  0);
        public static readonly Vector SE = new Vector(+1, +1);
        public static readonly Vector S  = new Vector( 0, +1);
        public static readonly Vector SW = new Vector(-1, +1);
        public static readonly Vector W  = new Vector(-1,  0);
        public static readonly Vector[] AllUnits = new[] { NW, N, NE, E, SE, S, SW, W };

        /// <summary>
        /// Returns the straight-line distance between two points, rounded up.
        /// </summary>
        /// <param name="a">One point.</param>
        /// <param name="b">The other point.</param>
        /// <returns>The straight-line distance between two points, rounded up.</returns>
        public double Distance => Sqrt(X * X + Y * Y);

        /// <summary>
        /// This vector's X distance.
        /// </summary>
        public int X;

        /// <summary>
        /// This vector's Y distance.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initialises a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">This vector's X distance.</param>
        /// <param name="y">This vector's Y distance.</param>
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj != null
                && typeof(Vector) == obj.GetType()
                && this == (Vector)obj;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => unchecked((Y + int.MinValue) ^ X);

        public static bool operator ==(Vector left, Vector right) => left.X == right.X && left.Y == right.Y;
        public static bool operator !=(Vector left, Vector right) => !(left == right);

        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);

        public static Vector operator -(Vector left, Vector right) => new Vector(left.X - right.X, left.Y - right.Y);
    }
}
