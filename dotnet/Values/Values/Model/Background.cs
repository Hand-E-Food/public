namespace Values.Model;

/// <summary>
/// A player background card.
/// </summary>
public record Background
{
    /// <summary>
    /// This background's title.
    /// </summary>
    public required string Title;
    /// <summary>
    /// The resources gained in each decade.
    /// </summary>
    public required Item[] Gains;
}
