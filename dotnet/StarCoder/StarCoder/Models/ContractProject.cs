namespace StarCoder.Models;

/// <summary>
/// A contractual project that pays money.
/// </summary>
public class ContractProject : FeatureProject, IProject
{
    public override ProjectOutcome GetOutcome()
    {
        if (State == ProjectState.Completed)
            return new() { Cash = CashReward };
        else if (State == ProjectState.Abandoned || State == ProjectState.Failed)
            return new() { Cash = CashPenalty };
        else
            return ProjectOutcome.None;
    }

    public int CashPenalty { get; protected set; } = 0;

    public int CashReward { get; protected set; } = 0;

    public required string Name { get; init; }
}
