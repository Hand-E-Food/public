namespace StarCoder;

/// <summary>
/// A game's settings.
/// </summary>
public class GameSettings
{
    /// <summary>
    /// Burnout does not change when the coder has this many cards in their hand.
    /// Burnout increases if the coder has less than this many cards.
    /// Burnout reduces if the coder has more than this many cards.
    /// </summary>
    public int BurnoutThreshold { get; set; } = 5;

    /// <summary>
    /// The number of cards in the coder's deck.
    /// </summary>
    public int DeckSize { get; set; } = 15;

    /// <summary>
    /// The number of cards the coder draws each turn.
    /// </summary>
    public int DrawPerTurn { get; set; } = 6;

    /// <summary>
    /// The maximum number of cards the coder can have in their hand.
    /// </summary>
    public int MaximumHandSize { get; set; } = 10;

    /// <summary>
    /// The maximum number of simultaneous active projects the coder can have.
    /// </summary>
    public int MaximumProjectCount { get; set; } = 5;
}
