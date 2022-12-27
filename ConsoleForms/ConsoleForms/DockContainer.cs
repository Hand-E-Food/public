namespace ConsoleForms
{
    /// <summary>
    /// A control that docks its children to its edges.
    /// </summary>
    public class DockContainer : Container
    {
        public override void PerformLayout()
        {
            var region = Bounds;
            foreach (var child in Children)
            {
                if (child.WidthAnchor != null && child.HeightAnchor != null)
                    throw new InvalidOperationException($"Children of a {nameof(DockContainer)} cannot have both a {nameof(WidthAnchor)} and a {nameof(HeightAnchor)}.");

                if (child.WidthAnchor == null)
                {
                    child.SetHorizontalAnchor(region.Left, null, region.Right);
                    child.SetVerticalAnchor(
                        child.TopAnchor != null ? region.Top : null,
                        child.HeightAnchor,
                        child.BottomAnchor != null ? region.Bottom : null
                    );
                    if (child.TopAnchor == null)
                        region.Bottom -= child.Height;
                    else if (child.BottomAnchor == null)
                        region.Top += child.Height;
                    else
                        region = Rectangle.Empty;
                }
                else
                {
                    child.SetVerticalAnchor(region.Top, null, region.Bottom);
                    child.SetHorizontalAnchor(
                        child.LeftAnchor != null ? region.Left : null,
                        child.WidthAnchor,
                        child.RightAnchor != null ? region.Right : null
                    );
                    if (child.LeftAnchor == null)
                        region.Right -= child.Width;
                    else if (child.RightAnchor == null)
                        region.Left += child.Width;
                    else
                        region = Rectangle.Empty;
                }
            }
            base.PerformLayout();
        }
    }
}
