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

        public static Location operator +(Location l, Direction d) => new Location(l.X + d.DeltaX, l.Y + d.DeltaY);
        public static Location operator -(Location l, Direction d) => new Location(l.X - d.DeltaX, l.Y - d.DeltaY);
    }
}
