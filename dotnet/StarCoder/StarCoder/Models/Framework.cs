namespace StarCoder.Models;

/// <summary>
/// A framework that adds a production bonus to future work.
/// </summary>
/// <param name="quantity">The quantity of the bonus.</param>
/// <param name="feature">The feature the bonus is applied to.</param>
/// <param name="language">The language required to interface with this framework.</param>
public class Framework(int quantity, Feature feature, Language language) : FeatureProduction(quantity, feature, language)
{
    /// <summary>
    /// This framework's name.
    /// </summary>
    public string Name => $"{Language} {Feature} Framework";
}
