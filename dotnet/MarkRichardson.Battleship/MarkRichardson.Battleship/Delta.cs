namespace MarkRichardson.Battleship
{

    /// <summary>
    /// A predefined cell delta.
    /// </summary>
    public struct Delta
    {

        /// <summary>
        /// The X offset.
        /// </summary>
        public int DX;

        /// <summary>
        /// The Y offset.
        /// </summary>
        public int DY;

        /// <summary>
        /// Delta 1 in each of the cardianl directions.
        /// </summary>
        public static Delta[] CardinalDirections = new[] {
            new Delta { DX = -1, DY =  0 },
            new Delta { DX =  1, DY =  0 },
            new Delta { DX =  0, DY = -1 },
            new Delta { DX =  0, DY =  1 },
        };

        /// <summary>
        /// Delta 1 in each of the diagonal directions.
        /// </summary>
        public static Delta[] DiagonalDirections = new[] {
            new Delta { DX = -1, DY = -1 },
            new Delta { DX =  1, DY = -1 },
            new Delta { DX = -1, DY =  1 },
            new Delta { DX =  1, DY =  1 },
        };

        /// <summary>
        /// Initalises a new instance of the <see cref="Delta"/> structure.
        /// </summary>
        /// <param name="dx">The X offset.</param>
        /// <param name="dy">The Y offset.</param>
        public Delta(int dx, int dy)
        {
            DX = dx;
            DY = dy;
        }
    }
}
