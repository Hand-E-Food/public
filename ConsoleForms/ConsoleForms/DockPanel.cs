namespace ConsoleForms
{
    /// <summary>
    /// A panel that docks each control against an edge of this panel.
    /// </summary>
    public class DockPanel : ParentControl
    {
        protected override void PerformLayout()
        {
            var bounds = Bounds;
            foreach (var child in Children)
            {
                if (bounds.HasArea)
                    LayoutChild(child, bounds);
                else
                    child.Bounds = Rectangle.Empty;

                if (child.AnchorTop.HasValue && !child.AnchorBottom.HasValue) bounds.Top = child.Bounds.Bottom;
                if (child.AnchorBottom.HasValue && !child.AnchorTop.HasValue) bounds.Bottom = child.Bounds.Top;
                if (child.AnchorLeft.HasValue && !child.AnchorRight.HasValue) bounds.Left = child.Bounds.Right;
                if (child.AnchorRight.HasValue && !child.AnchorLeft.HasValue) bounds.Right = child.Bounds.Left;
            }
        }
    }
}
