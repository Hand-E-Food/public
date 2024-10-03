namespace Underground;

/// <summary>
/// An option stemming from a branch.
/// </summary>
/// <param name="outcome">This option's outcome.</param>
/// <param name="weight">This option's weight.</param>
public class BranchOption(Outcome outcome, int weight = 0)
{
    /// <summary>
    /// This option's outcome.
    /// </summary>
    public Outcome Outcome { get; } = outcome;

    /// <summary>
    /// This option's weight.
    /// </summary>
    public int Weight { get; set; } = weight;
}
