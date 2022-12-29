namespace ConsoleForms
{
    /// <summary>
    /// Lays out its children starting from the top of this control.
    /// </summary>
    public class ScrollList : ParentControl
    {
        /// <summary>
        /// The <see cref="ScrollOffset"/> that displays the first child at the top of the container.
        /// </summary>
        public int ScrollTop => 0;

        /// <summary>
        /// The <see cref="ScrollOffset"/> that displays the last child at the bottom of the container.
        /// </summary>
        public int ScrollBottom => Children.Sum(child => child.Bounds.Height) - Bounds.Height;

        /// <summary>
        /// The vertical position displayed at the top of this control.
        /// </summary>
        public int ScrollOffset
        {
            get => scrollOffset;
            set
            {
                scrollOffset = value;
                InvalidateLayout();
            }
        }
        private int scrollOffset = 0;

        protected override void PerformLayout()
        {
            var bounds = Bounds;
            bounds.Top = -ScrollOffset;
            foreach (var child in Children)
            {
                bounds.Bottom = bounds.Top + 1;
                LayoutChild(child, bounds);
                bounds.Bottom = bounds.Top
                    + child.GetDesiredHeight().GetValueOrDefault(1)
                    + child.AnchorTop.GetValueOrDefault(0)
                    + child.AnchorBottom.GetValueOrDefault(0);
                LayoutChild(child, bounds);
                bounds.Top = bounds.Bottom;
            }
        }
    }
}
