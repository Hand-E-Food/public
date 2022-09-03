namespace ColourBlind
{
    /// <summary>
    /// A 2-dimensional point.
    /// </summary>
    public struct PointD
    {
        /// <summary>
        /// This point's x coordinate.
        /// </summary>
        public double X;

        /// <summary>
        /// This point's y coordinate.
        /// </summary>
        public double Y;

        /// <summary>
        /// Initialises a new <see cref="PointD"/>.
        /// </summary>
        /// <param name="x">This point's x coordinate.</param>
        /// <param name="y">This point's y coordinate.</param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
