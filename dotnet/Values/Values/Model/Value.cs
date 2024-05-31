namespace Values.Model;

/// <summary>
/// A societial or personal value card.
/// </summary>
public record Value
{
    /// <summary>
    /// This value's title.
    /// </summary>
    public required string Title;
    /// <summary>
    /// This value's text.
    /// </summary>
    /// <inheritdoc cref="IImageGenerator.FormatText(string)" path="/remarks"/>
    public required string Text;
}
