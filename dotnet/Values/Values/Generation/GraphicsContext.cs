using Microsoft.Maui.Graphics;

namespace Values.Generation;

/// <summary>
/// Details the graphical state and formatting of a block of text.
/// </summary>
public class GraphicsContext
{
    /// <summary>
    /// The block's bounds.
    /// </summary>
    public required RectF Bounds;

    /// <summary>
    /// The canvas to draw to.
    /// </summary>
    public required ICanvas Canvas;

    /// <summary>
    /// The font to use.
    /// </summary>
    public required IFont Font;

    /// <summary>
    /// The font's size in px.
    /// </summary>
    public required float FontSize;

    /// <summary>
    /// The height of each line of text.
    /// </summary>
    public float LineHeight {
        get => lineHeight ??= Canvas.GetStringSize("0", Font, FontSize).Height;
        set => lineHeight = value;
    }
    private float? lineHeight;

    /// <summary>
    /// The vertical whitespace between each line.
    /// </summary>
    public float LineSpacing
    {
        get => lineSpacing ??= LineHeight * 0.2f;
        set => lineSpacing = value;
    }
    private float? lineSpacing;

    /// <summary>
    /// The vertical whitespace between paragraphs.
    /// </summary>
    public float ParagraphSpacing
    {
        get => paragraphSpacing ??= LineSpacing * 2f;
        set => paragraphSpacing = value;
    }
    private float? paragraphSpacing;

    /// <summary>
    /// The horizontal whitespace between each word.
    /// </summary>
    public float WordSpacing
    {
        get => wordSpacing ??= Canvas.GetStringSize(" ", Font, FontSize).Width;
        set => wordSpacing = value;
    }
    private float? wordSpacing;
}
