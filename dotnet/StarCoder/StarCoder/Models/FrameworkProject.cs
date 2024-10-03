namespace StarCoder.Models;

/// <summary>
/// A project whose reward enhances the coder's ability.
/// </summary>
public class FrameworkProject : FeatureProject, IProject
{
    /// <summary>
    /// The framework awarded by completing this project.
    /// </summary>
    public required Framework Framework { get; init; }

    public override ProjectOutcome GetOutcome()
    {
        if (State == ProjectState.Completed)
            return new() { Framework = Framework };
        else
            return ProjectOutcome.None;
    }

    public string Name => Framework.Name;
}
