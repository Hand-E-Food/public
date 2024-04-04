using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Removes all undefined roads from the map.
/// </summary>
public class RoadFinalizer : SingleStepMapGeneratorComponent
{
    /// <summary>
    /// Gets or sets the road quality to lay.
    /// </summary>
    public RoadQuality LaidRoadQuality { get; set; } = RoadQuality.Undefined;

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // Set all undefined roads to no roads.
        foreach (var road in Map.Roads.Where(road => road.Quality == RoadQuality.Undefined))
            road.Quality = LaidRoadQuality;

        base.Step();
    }
}
