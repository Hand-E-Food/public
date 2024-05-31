namespace Values.Model;

/// <summary>
/// A decade card.
/// </summary>
public record Decade
{
    /// <summary>
    /// This decade's title.
    /// </summary>
    public required string Title;
    /// <summary>
    /// This card's text.
    /// </summary>
    /// <inheritdoc cref="IImageGenerator.FormatText(string)" path="/remarks"/>
    public required string Text;

}
