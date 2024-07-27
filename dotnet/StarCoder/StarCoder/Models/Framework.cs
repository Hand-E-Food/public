using System.Diagnostics.CodeAnalysis;

namespace StarCoder.Models;

/// <summary>
/// A framework that adds a production bonus to future work.
/// </summary>
/// <param name="production">The quantity of the bonus.</param>
/// <param name="feature">The feature the bonus is applied to.</param>
/// <param name="language">The language required to interface with this framework.</param>
[method: SetsRequiredMembers]
public class Framework(int production, Feature feature, Language language) : FeatureProduction(production, feature)
{
    /// <summary>
    /// This framework's language.
    /// </summary>
    public Language Language { get; } = language;

    /// <summary>
    /// This framework's name.
    /// </summary>
    public string Name => $"{Language} {Feature} Framework";
}
