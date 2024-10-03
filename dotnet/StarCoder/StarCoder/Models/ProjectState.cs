namespace StarCoder.Models;

/// <summary>
/// The state of a project.
/// </summary>
public enum ProjectState
{
    /// <summary>
    /// The project is advertised and no one has accepted it.
    /// </summary>
    Offered,

    /// <summary>
    /// The project was abadoned by the advertiser due to lack of interest.
    /// </summary>
    Expired,

    /// <summary>
    /// The coder has accepted the project contract and it is incomplete.
    /// </summary>
    Accepted,

    /// <summary>
    /// The coder has completed the project contract and can be rewarded.
    /// </summary>
    Completed,

    /// <summary>
    /// The coder has formally abandoned the project. There may be penalties.
    /// </summary>
    Abandoned,

    /// <summary>
    /// The coder has failed to complete the project within the contractural requirements.
    /// </summary>
    Failed,
}
