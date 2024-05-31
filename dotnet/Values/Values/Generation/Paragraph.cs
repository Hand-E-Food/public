using Microsoft.Maui.Graphics;

namespace Values.Generation;

/// <summary>
/// A collection of words after which is a new line.
/// </summary>
/// <param name="words">This paragraph's words.</param>
public abstract class Paragraph(IReadOnlyList<Word> words)
{ 
    /// <summary>
    /// This paragraph's bounds.
    /// </summary>
    public RectF Bounds;

    /// <summary>
    /// This paragraph's words.
    /// </summary>
    public readonly IReadOnlyList<Word> Words = words;

    /// <summary>
    /// Calculates and sets this paragraph's size.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    internal abstract void CalculateSize(GraphicsContext context);

    /// <summary>
    /// Calculates and sets this location of this paragraph's words.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    internal abstract void CalculateLocation(GraphicsContext context);

    /// <summary>
    /// Draws this paragraph to the canvas.
    /// </summary>
    /// <param name="context">The graphical context to use.</grapics>
    internal void Draw(GraphicsContext context)
    {
        foreach (var word in Words)
            word.Draw(context);
    }
}

/// <summary>
/// A paragraph of linear text.
/// </summary>
/// <param name="words">This paragraph's words.</param>
public class LinearParagraph(IReadOnlyList<Word> words) : Paragraph(words)
{
    /// <summary>
    /// The paragraph's words grouped into lines.
    /// </summary>
    protected readonly List<List<Word>> Lines = [];

    internal override void CalculateSize(GraphicsContext context)
    {
        Lines.Clear();
        if (Words.Count == 0) return;
        List<Word> line = [];
        Lines.Add(line);
        float width = 0;
        foreach (var word in Words)
        {
            word.CalculateSize(context);
            if (word.SpaceBefore && line.Count > 0) width += context.WordSpacing;
            width += word.Bounds.Width;
            if (width > context.Bounds.Width)
            {
                Lines.Add(line = []);
                width = word.Bounds.Width;
            }
            line.Add(word);
        }
        Bounds.Width = context.Bounds.Width;
        Bounds.Height = Lines.Count * (context.LineHeight + context.LineSpacing) - context.LineSpacing;
    }

    internal override void CalculateLocation(GraphicsContext context)
    {
        float y = Bounds.Y;
        foreach (var line in Lines)
        {
            float lineWidth = line.Last().Bounds.Right - line.First().Bounds.Left;
            float x = (Bounds.Width - lineWidth) / 2 + Bounds.Left;
            bool firstWord = true;
            foreach (var word in Words)
            {
                if (firstWord) firstWord = false;
                else if (word.SpaceBefore) x += context.WordSpacing;

                word.Bounds.X = x;
                word.Bounds.Y = y;
                word.CalculateLocation(context);
                x += word.Bounds.Width;
            }
            y += context.LineHeight + context.LineSpacing;
        }
    }
}

/// <summary>
/// A paragraph of tabulated text.
/// </summary>
public class TableParagraph : Paragraph
{
    /// <summary>
    /// Creates a paragraph of tabulated text.
    /// </summary>
    public TableParagraph()
    : base(FlattenWords())
    {
        throw new NotImplementedException(); //TODO
    }

    private static IReadOnlyList<Word> FlattenWords()
    {
        throw new NotImplementedException(); //TODO
    }

    internal override void CalculateLocation(GraphicsContext context)
    {
        throw new NotImplementedException(); //TODO
    }

    internal override void CalculateSize(GraphicsContext context)
    {
        throw new NotImplementedException(); //TODO
    }
}
