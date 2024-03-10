using System.Diagnostics;
using ConsoleForms;

namespace ConsoleForms
{
    /// <summary>
    /// A graphics manager.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Graphics
    {
        private string DebuggerDisplay => $"ClipRegion = {ClipRegion}, Cursor = {CursorX},{CursorY}";

        /// <summary>
        /// Creates a new <see cref="Graphics"/>.
        /// </summary>
        /// <param name="bitmap">The bitmap to draw on.</param>
        /// <param name="clipRegion">The region to allow drawing on.</param>
        public Graphics(Bitmap bitmap, Rectangle clipRegion)
        {
            this.bitmap = bitmap;
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
            new(bitmap, Rectangle.Intersection(ClipRegion, region) ?? Rectangle.Empty);

        /// <summary>
        /// Clears the clip region.
        /// </summary>
        /// <param name="backgroundColor">The background color.</param>
        public void Clear(ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            if (!ClipRegion.HasArea) return;
            for (int y = ClipRegion.Top; y < ClipRegion.Bottom; y++)
                for (int x = ClipRegion.Left; x < ClipRegion.Right; x++)
                    bitmap.Pixels[x, y] = new Pixel(' ', backgroundColor: backgroundColor);
        }

        /// <summary>
        /// Sets the current cursor position.
        /// </summary>
        /// <param name="point">The cursor position.</param>
        public void SetCursorPosition(Point point) => SetCursorPosition(point.X, point.Y);

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
        /// The current position of the cursor.
        /// </summary>
        public Point Cursor => new(CursorX, CursorY);

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
                    bitmap.Pixels[CursorX, CursorY] = new Pixel(text[i], coloredText.ForegroundColor, coloredText.BackgroundColor);
                CursorX++;
                i++;
            }
        }

        /// <summary>
        /// Writes the specified bitmap to the current cursor position.
        /// </summary>
        /// <param name="bitmap">The bitmap to write.</param>
        public void Write(Bitmap bitmap) => throw new NotImplementedException();

        private readonly Bitmap bitmap;
    }
}
