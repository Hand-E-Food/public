namespace StarCoder.Models;

/// <summary>
/// A project that requires a specific quantity of features.
/// </summary>
public abstract class FeatureProject : Project
{
    /// <summary>
    /// The features required to complete this project.
    /// </summary>
    public required ICollection<FeatureRequirement> Features { get; init; }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="production"/> must be for a specific feature, not 'any' feature.</exception>
    public override void Produce(FeatureProduction production)
    {
        if (production.IsAnyFeature)
            throw new ArgumentException("Must produce a specific feature, not 'any' feature.");

        foreach (var requirement in Features)
            production = requirement.Produce(production);

        if (Features.All(feature => feature.IsCompleted))
            State = ProjectState.Completed;
    }
}
