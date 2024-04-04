using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;

namespace RandomVectorMap.Generation;

/// <summary>
/// 
/// </summary>
public class ServiceProvider : MapGeneratorComponent
{
    /// <summary>
    /// Gets the collection of road qualities that may be traversed.
    /// </summary>
    /// <value>A collection of road qualities.</value>
    public HashSet<RoadQuality> AllowedRoadQualities { get; } = [];

    /// <summary>
    /// Gets the collection of settlement sizes that can provide service.
    /// </summary>
    /// <value>A collection of settlement sizes.</value>
    public HashSet<SettlementSize> AllowedSettlementSizes { get; } = [];

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && (junctionDistances!.Count <= 0 || junctionDistances.First().Distance < MaximumDistance);

    /// <summary>
    /// A collection of junctions and their distance from a service station.
    /// </summary>
    private SortedSet<JunctionDistance>? junctionDistances;

    /// <summary>
    /// Gets or sets the maximum distance between service providers.
    /// </summary>
    public double MaximumDistance { get; set; } = double.MaxValue;

    /// <summary>
    /// The junctions that provide service.
    /// </summary>
    private HashSet<Junction>? serviceJunctions;

    /// <summary>
    /// Gets or sets the settlement size to build.
    /// </summary>
    public SettlementSize SettlementSize { get; set; } = SettlementSize.Undefined;

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        serviceJunctions = FindServiceJunctions().ToHashSet();
        InitializeJunctionDistances();
    }

    /// <summary>
    /// Initialises the collection of junction distances.
    /// </summary>
    private void InitializeJunctionDistances()
    {
        // Initialise the collection.
        junctionDistances = [];
        // Find all junctions with no settlement and at least one road.
        var junctions = Map.Junctions.Where(junction => junction.Size == SettlementSize.Undefined && junction.Roads.Any(road => AllowedRoadQualities.Contains(road.Quality)));
        // Measure the distance from each junction to the nearest service station.
        Parallel.ForEach(junctions, junction =>
        {
            var distance = MeasureDistanceToService(junction);
            bool lockTaken = false;
            try
            {
                LockForInitializeJunctionDistances.Enter(ref lockTaken);
                junctionDistances.Add(new(junction, distance));
            }
            finally
            {
                LockForInitializeJunctionDistances.Exit();
            }
        });
    }
    private SpinLock LockForInitializeJunctionDistances = new();

    /// <summary>
    /// Finds all service junctions.
    /// </summary>
    /// <returns>A collection of service junctions.</returns>
    private IEnumerable<Junction> FindServiceJunctions() => Map.Junctions.Where(j => AllowedSettlementSizes.Contains(j.Size));

    /// <summary>
    /// Measures the distance from the specified junction to the nearest service station.
    /// </summary>
    /// <param name="junction">The junction to search from.</param>
    /// <returns>The distance from the junction to the nearest service station.</returns>
    private double MeasureDistanceToService(Junction junction)
    {
        JunctionToJunctionNavigator navigator = new([junction], serviceJunctions!);
        navigator.RoadQualities.AddRange(AllowedRoadQualities);
        navigator.Solve();

        if (navigator.BestRoute is null)
            throw new InvalidOperationException($"Unable to find a route from {junction.Name} to services.");
        return navigator.BestRoute.Length;
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        if (junctionDistances is null || serviceJunctions is null)
            throw new InvalidOperationException("Class is not initialized.");
        // Get the junction furthest from any service station.
        var junction = junctionDistances.First();
        junctionDistances.Remove(junction);
        // Remeasure the junction's distance from the nearest service station.
        var oldDistance = junction.Distance;
        JunctionToJunctionNavigator navigator = new([junction], serviceJunctions);
        navigator.RoadQualities.AddRange(AllowedRoadQualities);
        navigator.Solve();
        var newDistance = navigator.BestRoute!.Length;
        // Check if the junction is now closer to a service station than when last measured.
        if (newDistance == oldDistance)
        {   // If this junction is still the furthest from a service station...
            // Create a service station here.
            junction.Junction.Size = SettlementSize;
        }
        else
        {   // If this junction is closer to a service station than last measured...
            // Reinsert it into the collection.
            junction.Distance = newDistance;
            junctionDistances.Add(junction);
        }
        // Mark debugging information.
        junction.Junction.DebugColor = Color.Red;
        foreach (var road in navigator.BestRoute.Roads)
            road.DebugColor = Color.Red;
    }

    /// <summary>
    /// Information related to a junction.
    /// </summary>
    /// <param name="junction">The junction.</param>
    /// <param name="distance">The distance this junction is from any service station.</param>
    private class JunctionDistance(Junction junction, double distance) : IComparable<JunctionDistance>
    {
        /// <summary>
        /// Gets the junction.
        /// </summary>
        public Junction Junction { get; } = junction;

        /// <summary>
        /// Gets or sets the distance this junction is from any service station.
        /// </summary>
        public double Distance { get; set; } = distance;

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(JunctionDistance? other) => other is null ? 1 : other.Distance.CompareTo(this.Distance);

        /// <summary>
        /// Converts this object to a Junction.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>The object's junction.</returns>
        public static implicit operator Junction(JunctionDistance obj) => obj.Junction;
    }
}
