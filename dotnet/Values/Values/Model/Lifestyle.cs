namespace Values.Model;

/// <summary>
/// A lifestyle card.
/// </summary>
public record Lifestyle {
    /// <summary>
    /// The decade in which this lifestyle can be chosen.
    /// </summary>
    public required string Decade;
    /// <summary>
    /// This lifestyle's title.
    /// </summary>
    public required string Title;
    /// <summary>
    /// This lifestyle's subtitle.
    /// </summary>
    public string? Subtitle = null;
    /// <summary>
    /// This lifestyle's symbols.
    /// </summary>
    public Category[] Categories = [];
    /// <summary>
    /// True if a player can only have one lifestyle with this title.
    /// False if this lifestyle can be combined with any other lifestyle.
    /// </summary>
    public bool Exclusive = false;
    /// <summary>
    /// The resources gained from this lifestyle.
    /// </summary>
    public Item[] Gains = [];
    /// <summary>
    /// The resources spent maintaining this lifestyle.
    /// </summary>
    public Item[] Costs = [];
    /// <summary>
    /// The resource penalty for abandoning this lifestyle.
    /// </summary>
    public Item? AbandonmentCost = null;
}
