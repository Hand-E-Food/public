namespace ConsoleForms
{
    /// <summary>
    /// Lays out its children starting from the top of this control.
    /// </summary>
    public class TopAlignedListContainer : Container
    {
        public override void PerformLayout()
        {
            int topAnchor = 0;
            foreach (var child in Children)
            {
                child.SetVerticalAnchor(topAnchor, child.HeightAnchor, null);
                topAnchor += child.Height;
            }
            base.PerformLayout();
        }

        protected override void AddControl(Control control)
        {
            if (control.HeightAnchor == null) throw new ArgumentException($"control must have a {nameof(HeightAnchor)}.");
            base.AddControl(control);
        }
    }
}
