using System.Diagnostics.CodeAnalysis;

namespace StarCoder.Models;

/// <summary>
/// Measures a quantity of a feature that is produced.
/// </summary>
public class FeatureProduction
{
    /// <summary>
    /// Creates a feature production metric.
    /// </summary>
    public FeatureProduction() { }

    /// <summary>
    /// Creates a feature production metric.
    /// </summary>
    /// <param name="production">The quantity of this feature that is produced.</param>
    /// <param name="feature">The feature that is produced.</param>
    [SetsRequiredMembers]
    public FeatureProduction(int production, Feature feature)
    {
        Feature = feature;
        Quantity = production;
    }

    /// <summary>
    /// The feature that is produced.
    /// </summary>
    public required Feature Feature { get; init; }

    /// <summary>
    /// The quantity of this feature that is produced.
    /// </summary>
    public required int Quantity { get; init; }
}
