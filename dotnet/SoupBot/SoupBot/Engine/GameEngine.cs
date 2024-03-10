using SoupBot.Mapping;
using SoupBot.Models;

namespace SoupBot.Engine;

/// <summary>
/// Runs the game.
/// </summary>
public interface IGameEngine
{
    /// <summary>
    /// Causes all idle elements of the map to act.
    /// </summary>
    /// <returns>The time at which this should next occur.</returns>
    DateTime Act();
}

public class GameEngine : IGameEngine
{
    private readonly Map map;

    public GameEngine(Map map)
    {
        this.map = map;
    }

    public DateTime Act()
    {
        DateTime now = DateTime.UtcNow;
        DateTime next = DateTime.MaxValue;
        foreach (Robot robot in map.Robots)
        {
            if (robot.BusyUntil <= now)
                robot.Act();
            if (robot.BusyUntil < next)
                next = robot.BusyUntil;
        }
        if (next <= now)
            next = now.AddMilliseconds(30);

        return next;
    }
}
