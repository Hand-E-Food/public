using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// The base class for all road laying classes.
/// </summary>
public abstract class RoadLayer : MapGeneratorComponent
{
    /// <summary>
    /// Gets a collection of allowed road qualities to form routes upon.
    /// </summary>
    /// <value>A collection of road qualities.</value>
    public HashSet<RoadQuality> AllowedRoadQualities { get; } = [];

    /// <summary>
    /// Gets or sets the road quality to lay.
    /// </summary>
    public RoadQuality LaidRoadQuality { get; set; } = RoadQuality.Undefined;

    /// <summary>
    /// Lays a road along a set of road segments.
    /// </summary>
    /// <param name="roads">The road segments along which to lay the road.</param>
    protected void LayRoad(Road[] roads)
    {
        foreach (var road in roads)
            if (road.Quality < LaidRoadQuality)
                road.Quality = LaidRoadQuality;
    }
}
