using System.Diagnostics.CodeAnalysis;

namespace ConsoleForms
{
    /// <summary>
    /// A visual control that exists inside another control.
    /// </summary>
    public class Control : IControl
    {
        /// <summary>
        /// Set's this control's X coordinates. Exactly one parameter must be null.
        /// </summary>
        /// <param name="leftAnchor">The distance between this control and its parent's left edge.</param>
        /// <param name="widthAnchor">This control's width.</param>
        /// <param name="rightAnchor">The distance between this control and its parent's right edge.</param>
        /// <returns>This object for method chaining.</returns>
        /// <exception cref="ArgumentException">Exactly one dimension must be null.</exception>
        public Control SetHorizontalAnchor(int? leftAnchor, int? widthAnchor, int? rightAnchor)
        {
            if (new[] { leftAnchor, widthAnchor, rightAnchor }.Count(x => x == null) != 1)
                throw new ArgumentException("Exactly one dimension must be null.");
            this.LeftAnchor = leftAnchor;
            this.WidthAnchor = widthAnchor;
            this.RightAnchor = rightAnchor;
            PerformLayout();
            return this;
        }

        /// <summary>
        /// Set's this control's Y coordinates. Exactly one parameter must be null.
        /// </summary>
        /// <param name="topAnchor">The distance between this control and its parent's top edge.</param>
        /// <param name="heightAnchor">This control's height.</param>
        /// <param name="bottomAnchor">The distance between this control and its parent's bottom edge.</param>
        /// <returns>This object for method chaining.</returns>
        /// <exception cref="ArgumentException">Exactly one dimension must be null.</exception>
        public Control SetVerticalAnchor(int? topAnchor, int? heightAnchor, int? bottomAnchor)
        {
            if (new[] { topAnchor, heightAnchor, bottomAnchor }.Count(x => x == null) != 1)
                throw new ArgumentException("Exactly one dimension must be null.");
            this.TopAnchor = topAnchor;
            this.HeightAnchor = heightAnchor;
            this.BottomAnchor = bottomAnchor;
            PerformLayout();
            return this;
        }

        public int Left => AssertParent && LeftAnchor != null ? Parent.Left + LeftAnchor.Value : Right - Width;
        public int? LeftAnchor { get; private set; } = null;

        public int Top => AssertParent && TopAnchor != null ? Parent.Top + TopAnchor.Value : Bottom - Height;
        public int? TopAnchor { get; private set; } = null;

        public int Right => AssertParent && RightAnchor != null ? Parent.Right - RightAnchor.Value : Left + Width;
        public int? RightAnchor { get; private set; } = null;

        public int Bottom => AssertParent && BottomAnchor != null ? Parent.Bottom - BottomAnchor.Value : Top + Height;
        public int? BottomAnchor { get; private set; } = null;

        public int Width => WidthAnchor ?? Right - Left;
        public int? WidthAnchor { get; private set; } = null;

        public int Height => HeightAnchor ?? Bottom - Top;
        public int? HeightAnchor { get; private set; } = null;

        public Rectangle Bounds => Rectangle.LTRB(Left, Top, Right, Bottom);

        /// <summary>
        /// Set's this control's visibility.
        /// </summary>
        /// <param name="value">True to make this control visible. False to hide this control.</param>
        /// <returns>This object for method chaining.</returns>
        public Control SetIsVisible(bool value)
        {
            isVisible = value;
            return this;
        }

        public bool IsVisible => isVisible && (Parent?.IsVisible ?? false);
        private bool isVisible = true;

        /// <summary>
        /// This control's parent control.
        /// </summary>
        public IControl? Parent { get; set; }

        /// <summary>
        /// Asserts that <see cref="Parent"/> is not null.
        /// </summary>
        /// <returns>true</returns>
        /// <exception cref="InvalidOperationException"><see cref="Parent"/> is null.</exception>
        [MemberNotNull(nameof(Parent))]
        private bool AssertParent
        {
            get
            {
                if (Parent == null) throw new InvalidOperationException($"{nameof(Parent)} must be not null.");
                return true;
            }
        }

        /// <summary>
        /// This control's default background color.
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Instructs the canvas to redraw this control.
        /// </summary>
        public void Invalidate() => Parent?.Invalidate(Bounds);

        void IControl.Invalidate(Rectangle region) => Parent?.Invalidate(region);

        /// <summary>
        /// Draws this control's foreground detail.
        /// </summary>
        /// <param name="graphics">The graphics controller.</param>
        public virtual void Draw(Graphics graphics)
        {
            DrawBackground(graphics);
        }

        /// <summary>
        /// Draws this control's background detail.
        /// </summary>
        /// <param name="graphics">The graphics controller.</param>
        protected virtual void DrawBackground(Graphics graphics)
        {
            graphics.Clear(BackgroundColor);
        }

        /// <summary>
        /// Positions all of this control's children.
        /// </summary>
        public virtual void PerformLayout() { }
    }
}