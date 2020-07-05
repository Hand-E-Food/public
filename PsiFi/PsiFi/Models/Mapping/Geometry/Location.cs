using System.Diagnostics;

namespace PsiFi.Models.Mapping.Geometry
{
    [DebuggerDisplay("{X},{Y}")]
    struct Location
    {
        public readonly int X;
        public readonly int Y;

        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the distance from this <see cref="Location"/> to the <paramref name="other"/> <see cref="Location"/>, rounded up.
        /// </summary>
        /// <param name="other">The <see cref="Location"/> to measure the distance to.</param>
        /// <returns>The distance from this <see cref="Location"/> to the <paramref name="other"/> <see cref="Location"/>, rounded up.</returns>
        public int DistanceFrom(Location other)
        {
            var dx = other.X - X;
            var dy = other.Y - Y;
            var r2 = dx * dx + dy * dy;
            int r = 0;
            while (r * r < r2) r++;
            return r;
        }

        public static Location operator +(Location l, Direction d) => new Location(l.X + d.DeltaX, l.Y + d.DeltaY);
        public static Location operator -(Location l, Direction d) => new Location(l.X - d.DeltaX, l.Y - d.DeltaY);
    }
}
