namespace ConsoleForms
{
    /// <summary>
    /// An in-memory console bitmap.
    /// </summary>
    public class Bitmap
    {
        /// <summary>
        /// Creates a new <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="width">This bitmap's width.</param>
        /// <param name="height">This bitmap's height.</param>
        public Bitmap(int width, int height)
        {
            Bounds = Rectangle.XYWH(0, 0, width, height);
            Pixels = new Pixel[width, height];
        }

        /// <summary>
        /// This bitmap's width.
        /// </summary>
        public int Width => Bounds.Width;

        /// <summary>
        /// This bitmap's height,
        /// </summary>
        public int Height => Bounds.Height;

        /// <summary>
        /// This bitmap's bounds.
        /// </summary>
        public Rectangle Bounds { get; }

        /// <summary>
        /// This bitmap's pixels.
        /// </summary>
        public Pixel[,] Pixels { get; }
    }
}
