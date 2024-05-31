using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace Values.Generation;

/// <summary>
/// Creates a pre-defined image that can be saved as a PNG file.
/// </summary>
public class CardPng
{
    /// <summary>
    /// This card's total height in mm.
    /// </summary>
    public const float Height = 89f;

    /// <summary>
    /// This card's total width in mm.
    /// </summary>
    public const float Width = 66f;

    private const int dpi = 300;
    private static readonly float scale = dpi / 25.4f;
    private readonly SkiaBitmapExportContext image = new((int)Math.Ceiling(Width * scale), (int)Math.Ceiling(Height * scale), scale, dpi, transparent: false);

    /// <summary>
    /// Gets this image's canvas.
    /// </summary>
    public ICanvas Canvas => image.Canvas;

    /// <summary>
    /// Writes this image to a file.
    /// </summary>
    /// <param name="path">The path to write the file to, including the file extension.</param>
    public void WriteToFile(string path) => image.WriteToFile(path);
}
