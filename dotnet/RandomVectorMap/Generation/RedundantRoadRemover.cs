using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;
using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Generation;

/// <summary>
/// Removes roads where alternate routes will suffice.
/// </summary>
public class RedundantRoadRemover : MapGeneratorComponent
{
    /// <summary>
    /// Gets or sets the ratio between a road and an alternate route that would make the road redundant.
    /// </summary>
    /// <value>The ratio between a road and an alternate route that would make the road redundant.</value>
    public double AlternateRouteRatio { get; set; }

    /// <summary>
    /// The index of the next road to process.
    /// </summary>
    /// <value>The current index of the Roads list.</value>
    private int index = 0;

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && index >= roads!.Count;

    /// <summary>
    /// The list of all roads sorted by longest first.
    /// </summary>
    /// <value>A list of roads.</value>
    private List<Road>? roads;

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    [MemberNotNull(nameof(roads))]
    public override void Initialize()
    {
        base.Initialize();

        // Collect a list of all roads, sorted by longest first.
        roads = Map.Roads.Where(road => road.Quality == RoadQuality.Undefined).ToList();
        roads.Sort((a, b) => b.Length.CompareTo(a.Length));
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        if (roads is null) throw new InvalidOperationException("Class is not initialized.");

        // Select the road to process.
        Road road = roads[index];

        // Find alternate routes to this road.
        JunctionToJunctionNavigator navigator = new(
            [road.Junctions.First()],
            [road.Junctions.Last()]
        )
        {
            Minimum = road.Length,
            Maximum = road.Length * AlternateRouteRatio,
            RoadQualities = {
                RoadQuality.Undefined,
                RoadQuality.Dirt,
                RoadQuality.Paved,
                RoadQuality.Highway
            },
        };
        navigator.Solve();
        // If an alternate route is found, ensure there is no road here.
        if (navigator.BestRoute is not null)
        {   // If an alternate route is found...
            // Ensure there is no road here.
            road.Quality = RoadQuality.None;
            road.DebugColor = Color.Red;
            foreach (var otherRoad in navigator.BestRoute.Roads)
            {
                otherRoad.DebugColor = Color.Blue;
            }
        }
        else
        {   // If no alternate route is found...
            // Keep this route.
            road.DebugColor = Color.Green;
        }

        // Move to the next road.
        index++;
    }
}
