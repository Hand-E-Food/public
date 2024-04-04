using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Generation;

/// <summary>
/// Lays driveways between settlements and main roads.
/// </summary>
public class DrivewayLayer : RoadLayer
{
    /// <summary>
    /// The settlement index counter.
    /// </summary>
    /// <value>An index in the Settlements array.</value>
    private int index = 0;

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && index >= settlements!.Length;

    /// <summary>
    /// The settlements to join.
    /// </summary>
    /// <value>An array of settlements.</value>
    private Junction[]? settlements;

    /// <summary>
    /// The list of settlement sizes to include.
    /// </summary>
    /// <value>A collection of settlement sizes.</value>
    public HashSet<SettlementSize> SettlementSizes { get; } = [];

    /// <summary>
    /// Gets the collection of road qualities that this road should connect to.
    /// </summary>
    public HashSet<RoadQuality> TargetRoadQualities { get; } = [];

    /// <summary>
    /// The list of target settlement sizes to include.
    /// </summary>
    /// <value>A collection of settlement sizes.</value>
    public HashSet<SettlementSize> TargetSettlementSizes { get; } = [];

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    [MemberNotNull(nameof(settlements))]
    public override void Initialize()
    {
        base.Initialize();

        settlements = Map.Junctions.Where(junction => SettlementSizes.Contains(junction.Size)).ToArray();
    }

    /// <summary>
    /// Lays a road between the two junctions.
    /// </summary>
    /// <param name="origin">The road's origin.</param>
    /// <param name="destination">The road's destination.</param>
    private void LayRoad(Junction origin)
    {
        Junction[] destinations = Map.Junctions.Where(junction => TargetSettlementSizes.Contains(junction.Size) || junction.Roads.Select(road => road.Quality).Intersect(TargetRoadQualities).Any()).ToArray();
        JunctionToJunctionNavigator navigator = new([origin], destinations);
        navigator.RoadQualities.AddRange(AllowedRoadQualities);

        navigator.Solve();
        if (navigator.BestRoute is null) return;

        LayRoad(navigator.BestRoute.Roads);

        foreach (var road in navigator.BestRoute.Roads)
            road.DebugColor = Color.Blue;
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        if (settlements is null) throw new InvalidOperationException("Class is not initialized.");
        LayRoad(settlements[index++]);
    }
}
