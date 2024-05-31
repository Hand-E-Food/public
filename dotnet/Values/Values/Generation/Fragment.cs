using Microsoft.Maui.Graphics;

namespace Values.Generation;

/// <summary>
/// A fragment of a word.
/// </summary>
public abstract class Fragment
{
    /// <summary>
    /// This fragment's bounds.
    /// </summary>
    public RectF Bounds;

    /// <summary>
    /// Calculates and sets this fragment's size.
    /// </summary>
    /// <param name="context">The graphical context to use.</param>
    internal abstract void CalculateSize(GraphicsContext context);

    /// <summary>
    /// Draws this fragment to the canvas.
    /// </summary>
    /// <param name="context">The graphical context to use.</grapics>
    internal abstract void Draw(GraphicsContext context);
}

/// <summary>
/// A word fragment that is a string.
/// </summary>
/// <param name="text">The fragment's text.</param>
public class StringFragment(string text) : Fragment
{
    /// <summary>
    /// This fragment's text.
    /// </summary>
    public readonly string Text = text;

    internal override void CalculateSize(GraphicsContext context)
    {
        Bounds.Size = context.Canvas.GetStringSize(Text, context.Font, context.FontSize);
    }

    internal override void Draw(GraphicsContext context)
    {
        context.Canvas.DrawString(Text, Bounds.Left, Bounds.Top, HorizontalAlignment.Left);
    }
}

/// <summary>
/// A word fragment that is an image.
/// </summary>
/// <param name="image">This fragment's image.</param>
public class ImageFragment(ImageFragment.GetImageFunction getImage) : Fragment
{
    /// <summary>
    /// A function that gets an image resized to the specified height.
    /// </summary>
    /// <param name="height">The desired image height.</param>
    /// <returns>An image resized to the specified height.</returns>
    public delegate IImage GetImageFunction(float height);

    /// <summary>
    /// This fragment's image.
    /// </summary>
    public readonly GetImageFunction GetImage = getImage;

    internal override void CalculateSize(GraphicsContext context)
    {
        float height = context.LineHeight;
        Bounds.Height = height;
        Bounds.Width = GetImage(height).Width;
    }

    internal override void Draw(GraphicsContext context)
    {
        var image = GetImage(Bounds.Height);
        context.Canvas.DrawImage(image, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
    }
}
