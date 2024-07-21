namespace StarCoder.Models;

/// <summary>
/// Measures a quantity of a feature that is produced.
/// </summary>
/// <param name="quantity">The quantity of this feature that is produced.</param>
/// <param name="feature">The feature that is produced.</param>
/// <param name="language">The specific language produced.</param>
public class FeatureProduction(int quantity, Feature feature, Language language)
{
    /// <summary>
    /// The quantity of this feature that is produced.
    /// </summary>
    public int Quantity { get; } = quantity;

    /// <summary>
    /// The feature that is produced.
    /// </summary>
    public Feature Feature { get; } = feature;

    /// <summary>
    /// True if this is a quantity of `any` feature.
    /// False if this is a quantity of a specific feature.
    /// </summary>
    public bool IsAnyFeature => Feature == Feature.Any;

    /// <summary>
    /// The specific language produced.
    /// </summary>
    public Language Language { get; } = language;

    /// <summary>
    /// Converts this quantity of 'any' feature into a specific feature.
    /// </summary>
    /// <param name="feature">The feature to convert to.</param>
    /// <returns>This quantity of the specified feature.</returns>
    /// <exception cref="InvalidOperationException">This is not a quantity of 'any' feature.</exception>
    public FeatureProduction For(Feature feature)
    {
        if (IsAnyFeature)
            return new(Quantity, feature, Language);

        throw new InvalidOperationException("Only a quantity of 'any' feature can be changed into a specific feature.");
    }
}
