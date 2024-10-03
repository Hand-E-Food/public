namespace StarCoder.Models;

/// <summary>
/// The results of a project that has ended.
/// </summary>
public sealed class ProjectOutcome
{
    /// <summary>
    /// A project outcome that results in no change.
    /// </summary>
    public static readonly ProjectOutcome None = new();

    /// <summary>
    /// The framework the coder receives.
    /// </summary>
    public Framework? Framework { get; init; }

    /// <summary>
    /// The amount of cash the coder receives (or loses.)
    /// </summary>
    public int? Cash { get; init; }

    /// <summary>
    /// The language training the coder receives.
    /// </summary>
    public Language? Language { get; init; }
}
