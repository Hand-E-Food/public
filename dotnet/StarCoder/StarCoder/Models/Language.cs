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
    /// The features this language can produce.
    /// </summary>
    public required ICollection<FeatureProduction> Features { get; init; }

    /// <summary>
    /// This language's roots. Knowing these languages helps to learn this language faster.
    /// </summary>
    public ICollection<Language> Roots { get; init; } = [];

    public override string ToString() => Name;
}
