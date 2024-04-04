using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Removes short roads from the map.
/// </summary>
public class ShortRoadRemover : SingleStepMapGeneratorComponent
{
    /// <summary>
    /// Gets or sets the minimum allowable road length.
    /// </summary>
    /// <value>The minimum allowable road length.</value>
    public double MinimumRoadLength { get; set; }

    /// <summary>
    /// Performs a single step of its task.
    /// </summary>
    public override void Step()
    {
        // Remove all roads shorter than the minimum length.
        foreach (var road in Map.Roads.Where(road => road.Length < MinimumRoadLength && road.Quality == RoadQuality.Undefined))
        {
            road.Quality = RoadQuality.None;
            road.DebugColor = Color.Red;
        }
        base.Step();
    }
}
