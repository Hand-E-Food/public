namespace StarCoder.Models;

/// <summary>
/// A programming language.
/// </summary>
public sealed class Language
{
    /// <summary>
    /// This language's name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// This language's abbreviation. Should be no more than 8 characters long.
    /// </summary>
    public required string Abbreviation { get; init; }

    /// <summary>
    /// The features this language can produce.
    /// </summary>
    public required IList<FeatureProduction> Production { get; init; }

    /// <summary>
    /// This language's roots. Knowing these languages helps to learn this language faster.
    /// </summary>
    public IList<Language> Roots { get; init; } = [];

    public override string ToString() => Name;
}
