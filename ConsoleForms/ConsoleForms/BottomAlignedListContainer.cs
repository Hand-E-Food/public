namespace ConsoleForms
{
    /// <summary>
    /// Lays out its children ending at the bottom of this control.
    /// </summary>
    public class BottomAlignedListContainer : Container
    {
        public override void PerformLayout()
        {
            int bottomAnchor = 0;
            foreach (var child in Enumerable.Reverse(Children))
            {
                child.SetVerticalAnchor(null, child.HeightAnchor, bottomAnchor);
                bottomAnchor += child.Height;
            }
            base.PerformLayout();
        }

        protected override void AddControl(Control control)
        {
            if (control.HeightAnchor == null) throw new ArgumentException($"Child control must have a {nameof(HeightAnchor)}.");
            base.AddControl(control);
        }
    }
}
