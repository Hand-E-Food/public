namespace StarCoder.Models;

/// <summary>
/// A project whose reward enhances the coder's ability.
/// </summary>
public class FrameworkProject : FeatureProject, IProject
{
    /// <summary>
    /// The framework rewarded by completing this project.
    /// </summary>
    public required Framework Framework { get; init; }

    public string Name => Framework.Name;
}
