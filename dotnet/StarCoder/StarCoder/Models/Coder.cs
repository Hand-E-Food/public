namespace StarCoder.Models;

/// <summary>
/// The player.
/// </summary>
public class Coder
{
    /// <summary>
    /// Creates a coder for a new game.
    /// </summary>
    /// <param name="settings">The game's settings.</param>
    public Coder(GameSettings settings)
    {
        Burnout = 0;
        Cash = 0;
        Frameworks = [];
        Languages = new(settings.DeckSize, settings.HandLimit);
        Language firstLanguage = new() {
            Name = "Read Documentation",
            Abbreviation = "ReadDocs",
            Production = [],
        };
        for (int i = settings.DeckSize; i > 0; i--)
            Languages.DrawPile.Add(firstLanguage);
        Languages.Draw(settings.HandLimit);
    }

    /// <summary>
    /// This coder's level of burnout.
    /// </summary>
    public int Burnout { get; set; }

    /// <summary>
    /// This coder's available cash.
    /// </summary>
    public int Cash { get; set; }

    /// <summary>
    /// This coder's completed frameworks.
    /// </summary>
    public List<Framework> Frameworks { get; }

    /// <summary>
    /// This coder's known languages.
    /// </summary>
    public Deck<Language> Languages { get; init; }
}
