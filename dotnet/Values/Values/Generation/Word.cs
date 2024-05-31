using Microsoft.Maui.Graphics;

namespace Values.Generation;

/// <summary>
/// A collection of fragments that should be presented as a continuous word.
/// </summary>
/// <param name="fragments">This word's fragments.</param>
public class Word(IReadOnlyList<Fragment> fragments)
{
    /// <summary>
    /// This word's bounds.
    /// </summary>
    public RectF Bounds;

    /// <summary>
    /// This word's fragments.
    /// </summary>
    public readonly IReadOnlyList<Fragment> Fragments = fragments;

    /// <summary>
    /// True to insert whitespace before this word.<br/>
    /// False to position this word directly next to the previous word.
    /// e.g. The second part of a hyphenated word.<br/>
    /// Default value is true.
    /// </summary>
    public bool SpaceBefore = true;

    /// <summary>
    /// Calculates and sets this word's size.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    internal void CalculateSize(GraphicsContext context)
    {
        foreach (var fragment in Fragments)
            fragment.CalculateSize(context);
        Bounds.Width = Fragments.Sum(fragment => fragment.Bounds.Width);
        Bounds.Height = context.LineHeight;
    }

    /// <summary>
    /// Calculates and sets this location of this word's fragments.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    internal void CalculateLocation(GraphicsContext context)
    {
        float x = Bounds.Left;
        foreach (var fragment in Fragments)
        {
            fragment.Bounds.X = x;
            fragment.Bounds.Y = Bounds.Top;
            x += fragment.Bounds.Width;
        }
    }

    /// <summary>
    /// Draws this word to the canvas.
    /// </summary>
    /// <param name="context">The graphical context to use.</grapics>
    internal void Draw(GraphicsContext context)
    {
        foreach (var fragment in Fragments)
            fragment.Draw(context);
    }
}
