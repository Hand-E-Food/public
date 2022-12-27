using System.Linq;

namespace ConsoleForms
{
    /// <summary>
    /// The top-level canvas to draw on.
    /// </summary>
    public class Canvas : IControl
    {
        /// <summary>
        /// The region that must be redrawn.
        /// </summary>
        private Rectangle? invalidatedRegion;

        /// <summary>
        /// Creates a new <see cref="Canvas"/>
        /// </summary>
        /// <param name="width">This canvas's width.</param>
        /// <param name="height">This canvas's height.</param>
        public Canvas(int width, int height)
        {
            try
            {
#pragma warning disable CA1416 // SupportedOSPlatform 
                Console.SetWindowSize(width, height + 2);
                Console.SetBufferSize(width, height + 2);
#pragma warning restore CA1416 // SupportedOSPlatform
            }
            catch (PlatformNotSupportedException)
            {
                if (Console.BufferWidth < width || Console.BufferHeight < height + 2)
                    throw new InvalidOperationException($"The console buffer must be at least {width} x {height + 2} characters.");
            }
            Width = width;
            Height = height;
            Pixels = new Pixel[Width, Height];
            Invalidate();
        }

        int IControl.Left => 0;
        int IControl.Top => 0;
        int IControl.Right => Width;
        int IControl.Bottom => Height;
        bool IControl.IsVisible => true;

        /// <summary>
        /// This canvas's width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// This canvas's height,
        /// </summary>
        public int Height { get; }
        
        /// <summary>
        /// This canvas's pixels.
        /// </summary>
        public Pixel[,] Pixels { get; }

        /// <summary>
        /// This canvas's child control.
        /// </summary>
        public Control? Child
        {
            get => child;
            set
            {
                if (child != null)
                    child.Parent = null;
                child = value;
                if (child != null)
                {
                    child.Parent = this;
                    child.SetHorizontalAnchor(0, Width, null);
                    child.SetVerticalAnchor(0, Height, null);
                }
                Invalidate();
            }
        }
        private Control? child = null;

        /// <summary>
        /// Instructs this canvas to redraw itself.
        /// </summary>
        public void Invalidate() =>
            invalidatedRegion = Rectangle.XYWH(0, 0, Width, Height);

        void IControl.Invalidate(Rectangle region)
        {
            if (invalidatedRegion == null)
                invalidatedRegion = region;
            else
                invalidatedRegion = Rectangle.Union(invalidatedRegion.Value, region);
        }

        /// <summary>
        /// Draws invalidated controls and writes this canvas to the console.
        /// </summary>
        public void Refresh()
        {
            if (invalidatedRegion == null) return;
            var region = invalidatedRegion.Value; 
            if (child == null)
            {
                var blank = new string(' ', region.Width);
                Console.ResetColor();
                for (int y = region.Top; y < region.Bottom; y++)
                {
                    Console.SetCursorPosition(y, region.Left);
                    Console.Write(blank);
                }
            }
            else
            {
                var graphics = new Graphics(this, region);
                child.Draw(graphics);
                for (int y = region.Top; y < region.Bottom; y++)
                {
                    Console.SetCursorPosition(y, region.Left);
                    var text = new List<char>(Width);
                    for (int x = region.Left; x < region.Right; x++)
                    {
                        var pixel = Pixels[x, y];
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
            invalidatedRegion = null;
        }

        /// <summary>
        /// Gets the next button press.
        /// </summary>
        /// <returns>The pressed button's action.</returns>
        public Action? GetButtonPress()
        {
            if (child == null) return null;
            var buttonActionsByKey = FindEnabledButtons(child)
                .ToDictionary(button => button.Key, button => button.Action);
            if (!buttonActionsByKey.Any()) return null;
            var validKeys = buttonActionsByKey.Keys.ToList();
            char key;
            Action action;
            do key = Key.Read();
            while (!buttonActionsByKey.TryGetValue(key, out action!));
            return action;
        }

        /// <summary>
        /// Finds all enabled buttons nested in the control.
        /// </summary>
        /// <param name="control">The control to search.</param>
        /// <returns>The enabled buttons nested in the control.</returns>
        private IEnumerable<Button> FindEnabledButtons(Control control)
        {
            if (control is Container container)
                return container.SelectMany(FindEnabledButtons);
            else if (control is Button button && button.IsEnabled)
                return new[] { button };
            else
                return Enumerable.Empty<Button>();
        }
    }
}
