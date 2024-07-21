namespace StarCoder.Models;

/// <summary>
/// The player.
/// </summary>
public class Coder
{
    /// <summary>
    /// This coder's level of burnout.
    /// </summary>
    public int Burnout { get; set; } = 0;

    /// <summary>
    /// This coder's available cash.
    /// </summary>
    public int Cash { get; set; } = 0;

    /// <summary>
    /// This coder's completed frameworks.
    /// </summary>
    public List<Framework> Frameworks { get; } = [];

    /// <summary>
    /// This coder's known languages.
    /// </summary>
    public Deck<Language> Languages { get; } = new();

    /// <summary>
    /// Applies burnout and replenished hand.
    /// </summary>
    public void NextWeek()
    {
        Burnout = Math.Min(0, Burnout + Languages.Hand.Count - 5);
        Languages.Draw(Math.Min(6, 10 - Languages.Hand.Count));
    }
}
