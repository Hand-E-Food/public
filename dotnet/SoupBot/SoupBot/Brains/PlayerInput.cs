namespace SoupBot.Brains;

/// <summary>
/// The inputs being given by the player.
/// </summary>
public interface IPlayerInput
{
    /// <summary>
    /// A number between -1 and +1 indicating the horizontal direction being input.
    /// </summary>
    public int Horizontal { get; }

    /// <summary>
    /// A number between -1 and +1 indicating the vertical direction being input.
    /// </summary>
    public int Vertical { get; }
}

/// <summary>
/// Summarises the player's inputs.
/// </summary>
public class PlayerInput : IPlayerInput
{
    /// <summary>
    /// True if the down button is pressed.
    /// </summary>
    public bool Down { get; set; }

    /// <summary>
    /// True if the left button is pressed.
    /// </summary>
    public bool Left { get; set; }

    /// <summary>
    /// True if the right button is pressed.
    /// </summary>
    public bool Right { get; set; }

    /// <summary>
    /// True if the up button is pressed.
    /// </summary>
    public bool Up { get; set; }

    public int Horizontal => (Left ? -1 : 0) + (Right ? 1 : 0);
    public int Vertical => (Up ? -1 : 0) + (Down ? 1 : 0);

    /// <summary>
    /// Clears all key states.
    /// </summary>
    public void Clear()
    {
        Down = false;
        Left = false;
        Right = false;
        Up = false;
    }
}
