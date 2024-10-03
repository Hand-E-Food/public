using StarCoder.Models;

namespace StarCoder;

/// <summary>
/// A game's current state.
/// </summary>
/// <param name="gameSettings">The game's settings.</param>
public class GameState
{
    /// <summary>
    /// Initialises the state of a new game.
    /// </summary>
    /// <param name="gameSettings">The game's settings.</param>
    public GameState(GameSettings gameSettings)
    {
        Coder = new(gameSettings);
        Plan = new(gameSettings.MaximumHandSize);
        Projects = new();
        Week = 0;
    }

    /// <summary>
    /// This game's coder.
    /// </summary>
    public Coder Coder { get; init; }

    /// <summary>
    /// The coder's planned production.
    /// </summary>
    public List<FeatureProduction> Plan { get; init; }

    /// <summary>
    /// The currently available projects.
    /// </summary>
    public List<IProject> Projects { get; init; }

    /// <summary>
    /// This game's current week.
    /// </summary>
    public int Week { get; set; }
}
