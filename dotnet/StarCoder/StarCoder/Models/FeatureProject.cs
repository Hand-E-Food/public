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
    public override void Produce(Language language, FeatureProduction production)
    {
        base.Produce(language, production);

        foreach (var requirement in Features)
            production = requirement.Produce(language, production);

        if (Features.All(feature => feature.IsCompleted))
            State = ProjectState.Completed;
    }
}
