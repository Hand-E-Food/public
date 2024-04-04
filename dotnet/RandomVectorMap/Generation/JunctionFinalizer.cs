using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Sets all undefined junctions to empty junctions.
/// </summary>
public class JunctionFinalizer : SingleStepMapGeneratorComponent
{
    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // Set all undefined junctions to no settlement.
        foreach (var junction in Map.Junctions.Where(junction => junction.Size == SettlementSize.Undefined))
            junction.Size = SettlementSize.None;

        // This task is finished.
        base.Step();
    }
}
