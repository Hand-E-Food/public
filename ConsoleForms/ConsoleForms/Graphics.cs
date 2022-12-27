namespace ConsoleForms
{
    /// <summary>
    /// A graphics manager.
    /// </summary>
    public class Graphics
    {
        private readonly Canvas canvas;

        /// <summary>
        /// Creates a new <see cref="Graphics"/>.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="clipRegion">The region to allow drawing on.</param>
        public Graphics(Canvas canvas, Rectangle clipRegion)
        {
            this.canvas = canvas;
            ClipRegion = clipRegion;
        }

        /// <summary>
        /// The region that can be drawn to.
        /// </summary>
        public Rectangle ClipRegion { get; }

        /// <summary>
        /// Creates a new <see cref="Graphics"/> object with a tighter clip region.
        /// </summary>
        /// <param name="region">The new clip region.</param>
        /// <returns>A <see cref="Graphics"/> object.</returns>
        public Graphics CreateClipRegion(Rectangle region) =>
            new(canvas, Rectangle.Intersection(ClipRegion, region) ?? Rectangle.Empty);

        /// <summary>
        /// Clears the clip region.
        /// </summary>
        /// <param name="backgroundColor">The background color.</param>
        public void Clear(ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            if (!ClipRegion.HasArea) return;
            for (int y = ClipRegion.Top; y < ClipRegion.Bottom; y++)
                for (int x = ClipRegion.Left; x < ClipRegion.Right; x++)
                    canvas.Pixels[x, y] = new Pixel(' ', backgroundColor: backgroundColor);
        }

        /// <summary>
        /// Sets the current cursor position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public void SetCursorPosition(int x, int y)
        {
            CursorX = x;
            CursorY = y;
        }

        /// <summary>
        /// The current X position of the cursor.
        /// </summary>
        public int CursorX { get; set; } = 0;

        /// <summary>
        /// The current Y position of the cursor.
        /// </summary>
        public int CursorY { get; set; } = 0;

        /// <summary>
        /// Writes the specified text at the current cursor position.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foregroundColor">The text's foreground color.</param>
        /// <param name="backgroundColor">The text's background color.</param>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is null.</exception>
        public void Write(string text, ConsoleColor foregroundColor, ConsoleColor backgroundColor) =>
            Write(new ColoredText(text, foregroundColor, backgroundColor));

        /// <summary>
        /// Writes the specified text at the current cursor position.
        /// </summary>
        /// <param name="coloredText">The text to write.</param>
        public void Write(ColoredText coloredText)
        {
            if (CursorY < ClipRegion.Top || CursorY >= ClipRegion.Bottom) return;
            var text = coloredText.Text;
            int i = Math.Max(0, Math.Min(ClipRegion.Left - CursorX, text.Length));
            CursorX += i;
            while (i < text.Length && CursorX < ClipRegion.Right)
            {
                if (CursorX >= ClipRegion.Left)
                    canvas.Pixels[CursorX, CursorY] = new Pixel(text[i], coloredText.ForegroundColor, coloredText.BackgroundColor);
                CursorX++;
                i++;
            }
        }
    }
}
