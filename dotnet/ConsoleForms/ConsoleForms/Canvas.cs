namespace ConsoleForms
{
    /// <summary>
    /// The top-level canvas to draw on.
    /// </summary>
    public class Canvas : IParentControl
    {
        /// <summary>
        /// Creates a new <see cref="Canvas"/>.
        /// </summary>
        /// <param name="width">This canvas's width.</param>
        /// <param name="height">This canvas's height.</param>
        public Canvas(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            bounds = Rectangle.XYWH(0, 0, bitmap.Width, bitmap.Height);
            try
            {
#pragma warning disable CA1416 // SupportedOSPlatform 
                Console.SetWindowSize(bounds.Width, bounds.Height + 2);
                Console.SetBufferSize(bounds.Width, bounds.Height + 2);
#pragma warning restore CA1416 // SupportedOSPlatform
            }
            catch (PlatformNotSupportedException)
            {
                if (Console.BufferWidth < bitmap.Width || Console.BufferHeight < bounds.Height + 2)
                    throw new InvalidOperationException($"The console buffer must be at least {bounds.Width} x {bounds.Height + 2} characters.");
            }
            InvalidateDrawing();
        }

        Canvas? IParentControl.Canvas => this;

        public void InvalidateLayout()
        {
            if (child == null) return;
            child.Bounds = bounds;
            InvalidateDrawing();
        }

        /// <summary>
        /// Instructs this canvas to redraw itself.
        /// </summary>
        public void InvalidateDrawing() => invalidatedDrawingRegion = bounds;

        void IParentControl.InvalidateDrawing(Rectangle region) =>
            invalidatedDrawingRegion += region * bounds;

        /// <summary>
        /// The region that must be redrawn.
        /// </summary>
        private Rectangle? invalidatedDrawingRegion;

        /// <summary>
        /// Gets the next key press and handles it.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Canvas"/> <see cref="Child"/> is null.
        /// </exception>
        public void ReadUserInput()
        {
            if (child == null) throw new InvalidOperationException("Canvas does not have a child control.");
            child.ValidateLayout();
            Rectangle? updatedRegion = null;
            while (invalidatedDrawingRegion.HasValue)
            {
                updatedRegion += invalidatedDrawingRegion;
                var graphics = new Graphics(bitmap, invalidatedDrawingRegion.Value);
                invalidatedDrawingRegion = null;
                child.ValidateDrawing(graphics);
            }
            if (updatedRegion.HasValue) WritePixels(updatedRegion.Value);
            while (!child.HandleKey(Key.Read())) { }
        }

        /// <summary>
        /// This canvas's child control.
        /// </summary>
        public IChildControl? Child
        {
            get => child;
            set
            {
                if (child != null)
                    child.Parent = null;
                child = value;
                if (child != null)
                    child.Parent = this;
                InvalidateLayout();
            }
        }
        private IChildControl? child = null;

        /// <summary>
        /// Writes all pixels to the console.
        /// </summary>
        public void WritePixels(Rectangle region)
        {
            for (int y = region.Top; y < region.Bottom; y++)
            {
                Console.SetCursorPosition(region.Left, y);
                var text = new List<char>(bitmap.Width);
                for (int x = region.Left; x < region.Right; x++)
                {
                    var pixel = bitmap.Pixels[x, y];
                    if (text.Count == 0 || pixel.ForegroundColor != Console.ForegroundColor || pixel.BackgroundColor != Console.BackgroundColor)
                    {
                        if (text.Count > 0)
                        {
                            Console.Write(new string(text.ToArray()));
                            text.Clear();
                        }
                        Console.ForegroundColor = pixel.ForegroundColor;
                        Console.BackgroundColor = pixel.BackgroundColor;
                    }
                    text.Add(pixel.Character);
                }
                Console.Write(new string(text.ToArray()));
            }
        }

        private readonly Rectangle bounds;

        /// <summary>
        /// The bitmap containing the drawing.
        /// </summary>
        private readonly Bitmap bitmap;
    }
}
