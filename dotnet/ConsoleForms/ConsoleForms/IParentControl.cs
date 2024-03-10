namespace ConsoleForms
{
    /// <summary>
    /// A parent of a control.
    /// </summary>
    public interface IParentControl
    {
        /// <summary>
        /// Instructs this control to update the layout of its children.
        /// </summary>
        void InvalidateLayout();

        /// <summary>
        /// Instructs this control to redraw the specified region.
        /// </summary>
        /// <param name="region">The region to redraw.</param>
        void InvalidateDrawing(Rectangle region);

        /// <summary>
        /// This control's top-level canvas.
        /// </summary>
        Canvas? Canvas { get; }
    }
}
