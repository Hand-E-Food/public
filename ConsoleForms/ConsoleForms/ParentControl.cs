namespace ConsoleForms
{
    /// <summary>
    /// A control with children.
    /// </summary>
    public class ParentControl : Control, IParentControl
    {
        /// <summary>
        /// Creates a new <see cref="ParentControl"/>.
        /// </summary>
        public ParentControl()
        {
            Children = new(this);
        }

        public override void ValidateLayout()
        {
            base.ValidateLayout();
            foreach (var child in Children)
                child.ValidateLayout();
        }

        protected override void PerformLayout()
        {
            foreach (var child in Children)
                LayoutChild(child, Bounds);
        }

        /// <summary>
        /// Positions a child control.
        /// </summary>
        /// <param name="child">The child control to position.</param>
        /// <param name="bounds">The bounds to anchor the control to.</param>
        protected void LayoutChild(IChildControl child, Rectangle bounds)
        {
            var left = bounds.Left;
            var top = bounds.Top;
            var right = bounds.Right;
            var bottom = bounds.Bottom;
            if (child.AnchorLeft.HasValue) left += child.AnchorLeft.Value;
            if (child.AnchorTop.HasValue) top += child.AnchorTop.Value;
            if (child.AnchorRight.HasValue) right -= child.AnchorRight.Value;
            if (child.AnchorBottom.HasValue) bottom -= child.AnchorBottom.Value;
            child.Bounds = Rectangle.LTRB(left, top, right, bottom);

            var desiredWidth = child.GetDesiredWidth();
            if (desiredWidth.HasValue)
            {
                var width = desiredWidth.Value;
                if (child.AnchorLeft.HasValue)
                {
                    if (!child.AnchorRight.HasValue)
                        right = left + width;
                }
                else if (child.AnchorRight.HasValue)
                {
                    left = right - width;
                }
                else
                {
                    left += (right - left - width) / 2;
                    right = left + width;
                }
            }
            var desiredHeight = child.GetDesiredHeight();
            if (desiredHeight.HasValue)
            {
                var height = desiredHeight.Value;
                if (child.AnchorTop.HasValue)
                {
                    if (!child.AnchorBottom.HasValue)
                        bottom = top + height;
                }
                else if (child.AnchorBottom.HasValue)
                {
                    top = bottom - height;
                }
                else
                {
                    top += (bottom - top - height) / 2;
                    bottom = top + height;
                }
            }
            child.Bounds = Rectangle.LTRB(left, top, right, bottom);
        }

        public override int? GetDesiredWidth() => desiredWidth;
        /// <summary>
        /// Sets this control's desired width.
        /// </summary>
        /// <param name="value">The desired width.</param>
        protected void SetDesiredWidth(int? value)
        {
            if (desiredWidth == value) return;
            desiredWidth = value;
            Parent?.InvalidateLayout();
        }
        private int? desiredWidth = null;

        public override int? GetDesiredHeight() => desiredHeight;
        /// <summary>
        /// Sets this control's desired height.
        /// </summary>
        /// <param name="value">The desired height.</param>
        protected void SetDesiredHeight(int? value)
        {
            if (desiredHeight == value) return;
            desiredHeight = value;
            Parent?.InvalidateLayout();
        }
        private int? desiredHeight = null;

        protected override void Draw(Graphics graphics)
        {
            foreach (var child in Children)
            {
                var region = graphics.ClipRegion * child.Bounds;
                if (region?.HasArea != true) continue;
                child.ValidateDrawing(graphics.CreateClipRegion(region.Value));
            }
        }

        /// <summary>
        /// This control's children.
        /// </summary>
        protected ChildControls Children { get; }

        void IParentControl.InvalidateDrawing(Rectangle region)
        {
            if (Parent == null) return;
            var croppedRegion = region * Bounds;
            if (croppedRegion.HasValue)
                Parent.InvalidateDrawing(croppedRegion.Value);
        }
    }
}
