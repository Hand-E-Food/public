using SoupBot.Brains;

namespace SoupBot.Models;

public abstract class Robot
{
    /// <summary>
    /// This robots various brains.
    /// </summary>
    protected abstract IEnumerable<IBrain> Brains { get; }

    /// <summary>
    /// This robot cannot make another action until this time.
    /// </summary>
    public DateTime BusyUntil { get; private set; } = DateTime.MinValue;

    /// <summary>
    /// The duration required to perform activities.
    /// </summary>
    public ActivityDurations Duration { get; } = new()
    {
        Walk = TimeSpan.FromSeconds(0.5),
    };

    /// <summary>
    /// This robot's image.
    /// </summary>
    public abstract Image Image { get; }

    /// <summary>
    /// This robot's location.
    /// </summary>
    required public Point Location { get; set; }

    /// <summary>
    /// This robot's name.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Puts this robot in a busy state for the specified duration.
    /// </summary>
    /// <param name="duration">The duration of the action.</param>
    public void BeBusyFor(TimeSpan duration)
    {
        BusyUntil = DateTime.UtcNow + duration;
    }

    /// <summary>
    /// Causes this robot to act.
    /// </summary>
    /// <returns>True if this robot performed an activity. False if it did not.</returns>
    public bool Act() => Brains.Any(brain => brain.Act());
}
