namespace StarCoder.Models;

/// <summary>
/// A contractual project that pays money.
/// </summary>
public class ContractProject : FeatureProject, IProject
{
    public required string Name { get; init; }
}
