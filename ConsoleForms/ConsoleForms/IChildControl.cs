namespace ConsoleForms
{
    /// <summary>
    /// A control that can be a child of another control.
    /// </summary>
    public interface IChildControl
    {
        /// <summary>
        /// Ensures this control and all of its children are laid out correctly.
        /// </summary>
        void ValidateLayout();

        /// <summary>
        /// The distance this control should be from its parent's left edge.
        /// </summary>
        int? AnchorLeft { get; }
        
        /// <summary>
        /// The distance this control should be from its parent's top edge.
        /// </summary>
        int? AnchorTop { get; }
        
        /// <summary>
        /// The distance this control should be from its parent's bottom edge.
        /// </summary>
        int? AnchorRight { get; }
        
        /// <summary>
        /// The distance this control should be from its parent's right edge.
        /// </summary>
        int? AnchorBottom { get; }

        /// <summary>
        /// Gets this control's desired width based on its current height.
        /// </summary>
        int? GetDesiredWidth();

        /// <summary>
        /// Gets this control's desired height based on its current width.
        /// </summary>
        int? GetDesiredHeight();

        /// <summary>
        /// This control's parent.
        /// </summary>
        IParentControl? Parent { get; set; }

        /// <summary>
        /// This control's position.
        /// </summary>
        Rectangle Bounds { get; set; }

        /// <summary>
        /// Handles the specified key press.
        /// </summary>
        /// <param name="key">The pressed key.</param>
        /// <returns>True if the key was handled. Otherwise, false.</returns>
        bool HandleKey(char key);
        
        /// <summary>
        /// Ensures this control and all of its children are drawn correctly.
        /// </summary>
        /// <param name="graphics">The graphics controller to draw to.</param>
        void ValidateDrawing(Graphics graphics);
    }
}
