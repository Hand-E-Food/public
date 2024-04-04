using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;

namespace RandomVectorMap.Generation;

/// <summary>
/// 
/// </summary>
public class DistantCityRoadLayer : RoadLayer
{
    /// <summary>
    /// Gets or sets the ratio between a route and an alternate route that would make the route redundant.
    /// </summary>
    /// <value>The ratio between a route and an alternate route that would make the route redundant.</value>
    public double AlternateRouteRatio { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the distance between settlements within which a road will be laid regardless of
    /// other factors.
    /// </summary>
    /// <value>The maximum distance between settlements within which a road will be laid regardless of
    /// other factors.</value>
    public double CasualDistance { get; set; } = double.PositiveInfinity;

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && routes.Count <= 0;

    /// <summary>
    /// The list of routes between settlements.
    /// </summary>
    /// <value>A sorted list of routes between each settlement.</value>
    private readonly SortedSet<Route> routes = [];

    /// <summary>
    /// Gets a collection of settlement sizes that are included.
    /// </summary>
    /// <value>A collection of settlement sizes.</value>
    public HashSet<SettlementSize> SettlementSizes { get; } = [];

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        // Find all settlements of the appropriate size.
        var settlements = Map.Junctions.Where(junction => SettlementSizes.Contains(junction.Size)).ToArray();

        // Find routes between each pair of networks.  Multi-threaded.
        List<Tuple<Junction, Junction>> settlementPairs = [];
        for (int i = 0; i < settlements.Length; i++)
            for (int j = i + 1; j < settlements.Length; j++)
                settlementPairs.Add(Tuple.Create(settlements[i], settlements[j]));
        
        Parallel.ForEach(settlementPairs, (p => InitializeRoute(p.Item1, p.Item2)));
    }

    /// <summary>
    /// Finds a route between two road networks.
    /// </summary>
    /// <param name="origin">The origin junction.</param>
    /// <param name="target">The target junction.</param>
    /// <returns>The best route found between the two junctions.  Null if no route was found.</returns>
    private void InitializeRoute(Junction origin, Junction target)
    {
        // Find the best route between the two junctions.
        JunctionToJunctionNavigator navigator = new([origin], [target]);
        navigator.RoadQualities.AddRange(AllowedRoadQualities);
        navigator.Solve();
        // Record the best route, if any.
        var route = navigator.BestRoute;
        if (route is not null)
        {
            bool lockTaken = false;
            try
            {
                LockForInitializeRoute.Enter(ref lockTaken);
                routes.Add(route);
            }
            finally
            {
                LockForInitializeRoute.Exit();
            }
        }
    }
    private SpinLock LockForInitializeRoute = new();

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // Get the shortest linking route.
        var route = routes.First();
        routes.Remove(route);
        // Show the proposed route.
        foreach (var road in route.Roads)
        {
            road.DebugColor = Color.Blue;
        }
        // Check if the route is longer than a casual route.
        Route? alternateRoute = null;
        if (route.Length > CasualDistance)
        {   // If the route is longer than a casual route...
            // Check if there is an alternate route for the route.
            JunctionToJunctionNavigator navigator = new([route.Junctions.First()], [route.Junctions.Last()])
            {
                Maximum = route.Length * AlternateRouteRatio,
                RoadQualities = { LaidRoadQuality },
            };
            navigator.Solve();
            alternateRoute = navigator.BestRoute;
        }
        if (alternateRoute is null)
        {   // If there is no alternate route or this route is of casual distance...
            // Lay the road.
            LayRoad(route.Roads);
        }
        else
        {   // If there is an alternate route...
            // Show the alternate route.
            foreach (var road in alternateRoute.Roads)
                road.DebugColor = Color.Red;
        }
    }
}
