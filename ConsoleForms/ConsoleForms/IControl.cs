namespace ConsoleForms
{
    public interface IControl
    {

        /// <summary>
        /// This control's left position.
        /// </summmary>
        int Left { get; }

        /// <summary>
        /// This control's top position.
        /// </summmary>
        int Top { get; }

        /// <summary>
        /// This control's right position.
        /// </summmary>
        int Right { get; }

        /// <summary>
        /// This control's bottom position.
        /// </summmary>
        int Bottom { get; }

        /// <summary>
        /// This control's width.
        /// </summmary>
        int Width { get; }

        /// <summary>
        /// This control's height.
        /// </summmary>
        int Height { get; }

        /// <summary>
        /// True if this control is visible and should be drawn. Otherwise, false.
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Instructs the canvas to redraw the specified region.
        /// </summary>
        /// <param name="region">The region to redraw.</param>
        void Invalidate(Rectangle region);
    }
}
