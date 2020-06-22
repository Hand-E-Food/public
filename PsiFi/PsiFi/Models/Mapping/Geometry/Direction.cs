using System.Collections.Generic;
using System.Diagnostics;

namespace PsiFi.Models.Mapping.Geometry
{
    /// <summary>
    /// The possible directions.
    /// </summary>
    [DebuggerDisplay("{DeltaX},{DeltaY}")]
    struct Direction
    {
        public static readonly Direction N = new Direction( 0, -1);
        public static readonly Direction E = new Direction(+1,  0);
        public static readonly Direction S = new Direction( 0, +1);
        public static readonly Direction W = new Direction(-1,  0);

        public static readonly IEnumerable<Direction> All = new[] { N, E, S, W };

        public readonly int DeltaX;
        public readonly int DeltaY;

        private Direction(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }
    }
}
