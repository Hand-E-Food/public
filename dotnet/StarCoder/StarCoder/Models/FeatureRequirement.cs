namespace StarCoder.Models;

/// <summary>
/// Measures a quantity of a feature that is required.
/// </summary>
public class FeatureRequirement
{
    /// <summary>
    /// The feature that is required.
    /// </summary>
    public Feature Feature { get; }

    /// <summary>
    /// True if this requirement is completed.
    /// False if this requirement is still being produced.
    /// </summary>
    public bool IsCompleted => Produced >= Required;

    /// <summary>
    /// The specific language required, if any.
    /// </summary>
    public Language? Language { get; }

    /// <summary>
    /// The quantity of this feature that is produced.
    /// </summary>
    public int Produced { get; private set; } = 0;

    /// <summary>
    /// The quantity of this feature that is required.
    /// </summary>
    public int Required { get; }

    /// <summary>
    /// Creates a feature requirement for a contract.
    /// </summary>
    /// <param name="quantity">The quantity of this feature that is required.</param>
    /// <param name="feature">The feature that is required.</param>
    /// <param name="language">The specific language required, if any.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="quantity"/> is not positive.</exception>
    public FeatureRequirement(int quantity, Feature feature, Language? language = null)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Must be positive.");
        Feature = feature;
        Language = language;
        Required = quantity;
    }

    /// <summary>
    /// Adds production to this requirement.
    /// </summary>
    /// <param name="language">The language used to produce.</param>
    /// <param name="production">The production to add.</param>
    /// <returns>The remaining production.</returns>
    public FeatureProduction Produce(Language language, FeatureProduction production)
    {
        if (production.Feature != Feature) return production;
        if (Language is not null && language != Language) return production;
        int quantity = Math.Max(0, Math.Min(Required - Produced, production.Quantity));
        Produced += quantity;
        return new FeatureProduction(production.Quantity - quantity, production.Feature);
    }
}
