using Microsoft.Maui.Graphics;

namespace Values.Generation;

/// <summary>
/// A block of text.
/// </summary>
/// <param name="paragraphs">This block's paragraphs.</param>
public class Block(IReadOnlyList<Paragraph> paragraphs)
{
    /// <summary>
    /// This blocks bounds.
    /// </summary>
    public RectF Bounds;

    /// <summary>
    /// This block's paragraphs.
    /// </summary>
    public readonly IReadOnlyList<Paragraph> Paragraphs = paragraphs;

    /// <summary>
    /// Calculates the size of all paragraphs in this block.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    public void CalculateSize(GraphicsContext context)
    {
        foreach (var paragraph in Paragraphs)
            paragraph.CalculateSize(context);
        Bounds.Width = context.Bounds.Width;
        Bounds.Height = Paragraphs.Sum(paragraph => paragraph.Bounds.Height) + (Paragraphs.Count - 1) * context.ParagraphSpacing;
    }

    /// <summary>
    /// Calculates and sets this location of this block's paragraphs.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    public void CalculateLocation(GraphicsContext context)
    {
        Bounds.X = context.Bounds.Left;
        Bounds.Y = (context.Bounds.Height - Bounds.Height) / 2f + context.Bounds.Top;
        float y = Bounds.Y;
        foreach (var paragraph in Paragraphs)
        {
            paragraph.Bounds.X = 0;
            paragraph.Bounds.Y = y;
            y += paragraph.Bounds.Height + context.ParagraphSpacing;
            paragraph.CalculateLocation(context);
        }
    }

    /// <summary>
    /// Draws this block to the canvas.
    /// </summary>
    /// <param name="context">The graphical context to use.</grapics>
    internal void Draw(GraphicsContext context)
    {
        foreach (var paragraph in Paragraphs)
            paragraph.Draw(context);
    }
}
