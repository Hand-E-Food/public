using SoupBot.Models;

namespace SoupBot.Brains;

/// <summary>
/// Part of a robot's brain.
/// </summary>
public interface IBrain
{
    /// <summary>
    /// The robot controlled by this brain.
    /// </summary>
    public Robot Robot { get; set; }

    /// <summary>
    /// Chooses an action for the robot owning this brain.
    /// </summary>
    /// <returns>true if the robot became busy. false to try another brain.</returns>
    public bool Act();
}
