namespace ConsoleForms
{
    /// <summary>
    /// A control that displays something to the user.
    /// </summary>
    public abstract class Control : IChildControl
    {
        /// <summary>
        /// Instructs this control to lay out its children.
        /// </summary>
        public void InvalidateLayout() => isLayoutValid = false;

        /// <summary>
        /// Ensures this control and all of its children are laid out correctly.
        /// </summary>
        public virtual void ValidateLayout()
        {
            if (isLayoutValid) return;
            PerformLayout();
            InvalidateDrawing();
            isLayoutValid = true;
        }

        /// <summary>
        /// Lays out this control's children.
        /// </summary>
        protected virtual void PerformLayout() { }
        
        private bool isLayoutValid = false;

        public int? AnchorLeft
        {
            get => anchorLeft;
            set
            {
                if (anchorLeft == value) return;
                anchorLeft = value;
                Parent?.InvalidateLayout();
            }
        }
        private int? anchorLeft = null;

        public int? AnchorTop
        {
            get => anchorTop;
            set
            {
                if (anchorTop == value) return;
                anchorTop = value;
                Parent?.InvalidateLayout();
            }
        }
        private int? anchorTop = null;

        public int? AnchorRight
        {
            get => anchorRight;
            set
            {
                if (anchorRight == value) return;
                anchorRight = value;
                Parent?.InvalidateLayout();
            }
        }
        private int? anchorRight = null;

        public int? AnchorBottom
        {
            get => anchorBottom;
            set
            {
                if (anchorBottom == value) return;
                anchorBottom = value;
                Parent?.InvalidateLayout();
            }
        }
        private int? anchorBottom = null;

        public virtual int? GetDesiredWidth() => null;

        public virtual int? GetDesiredHeight() => null;

        public void ValidateDrawing(Graphics graphics)
        {
            DrawBackground(graphics);
            Draw(graphics);
        }

        /// <summary>
        /// Draws this control's background.
        /// </summary>
        /// <param name="graphics">The graphics controller to draw to.</param>
        protected virtual void DrawBackground(Graphics graphics)
        {
            graphics.Clear(BackgroundColor);
        }

        /// <summary>
        /// Draws the control's content.
        /// </summary>
        /// <param name="graphics">The graphics controller to draw to.</param>
        protected abstract void Draw(Graphics graphics);

        public Rectangle Bounds
        {
            get => bounds;
            set
            {
                if (bounds == value) return;
                bounds = value;
                InvalidateLayout();
            }
        }
        private Rectangle bounds = Rectangle.Empty;

        /// <summary>
        /// This control's default background color.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (backgroundColor == value) return;
                backgroundColor = value;
                InvalidateDrawing();
            }
        }
        private ConsoleColor backgroundColor = ConsoleColor.Black;

        /// <summary>
        /// Instructs this control to redraw.
        /// </summary>
        public void InvalidateDrawing() => Parent?.InvalidateDrawing(Bounds);

        public Canvas? Canvas => Parent?.Canvas;

        public IParentControl? Parent { get; set; } = null;

        public virtual bool HandleKey(char key) => false;
    }
}
