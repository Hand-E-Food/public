using RandomVectorMap.Mapping;
using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Generation;

/// <summary>
/// Lays rivers between biomes.
/// </summary>
public partial class RiverLayer : RoadLayer
{
    /// <summary>
    /// The object used to compare altitudes.
    /// </summary>
    private Comparer? altitudeComparer = null;

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && sourceJunctions!.Count <= 0;

    /// <summary>
    /// Gets a collection of the biomes where rivers can start.
    /// </summary>
    /// <value>A collection of biomes.</value>
    public HashSet<Biome> SourceBiomes { get; } = [];

    /// <summary>
    /// The list of viable source junctions.
    /// </summary>
    private SortedSet<Junction>? sourceJunctions = null;

    /// <summary>
    /// Gets a collection of the biomes where rivers can end.
    /// </summary>
    /// <value>A collection of biomes.</value>
    public HashSet<Biome> TargetBiomes { get; } = [];

    /// <summary>
    /// The list of viable target junctions.
    /// </summary>
    private HashSet<Junction>? targetJunctions = null;

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    [MemberNotNull(nameof(altitudeComparer), nameof(sourceJunctions), nameof(targetJunctions))]
    public override void Initialize()
    {
        base.Initialize();

        // Set the altitude of each junction.
        AltitudeMapper altitudeMapper = new(Map);
        altitudeMapper.CalculateAltitudes();
        altitudeComparer = new(altitudeMapper.Altitudes);

        // Find source junctions.
        var sourceJunctions =
            Map.Zones                                                   // Find all zones
            .Where(z => SourceBiomes.Contains(z.Biome))               // that have one of the source biomes,
            .SelectMany(z => z.Junctions)                             // return all junctions adjacent to the zone
            .Distinct();                                                // but only once each.
        this.sourceJunctions = new(sourceJunctions, altitudeComparer);

        // Find taret junctions.
        var targetJunctions =
            Map.AllZones                                                // Find all zones
            .Where(z => TargetBiomes.Contains(z.Biome))               // that have one of the target biomes,
            .SelectMany(z => z.Junctions)                             // return all junctions adjacent to the zone
            .Distinct();                                                // but only once each.
        this.targetJunctions = new(targetJunctions);
    }

    /// <summary>
    /// Randomly selects a junction from which to start the river.
    /// </summary>
    /// <returns>The junction at which to start the river.  Null if no viable candidate was found.</returns>
    [MemberNotNull(nameof(sourceJunctions))]
    private Junction SelectSourceJunction()
    {
        if (sourceJunctions is null) throw new InvalidOperationException("Class is not initialized.");
        // Select the lowest point to start the river.
        var result = sourceJunctions.First();
        // Show debug information.
        foreach (var junction in sourceJunctions!)
            junction.DebugColor = Color.Red;
        result.DebugColor = Color.Blue;
        // Return the selected source junction.
        return result;
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        if (altitudeComparer is null || sourceJunctions is null || targetJunctions is null)
            throw new InvalidOperationException("Class is not initialized.");
        // Select a source junction.
        var junction = SelectSourceJunction();
        // Remove all junctions of all surrounding zones from the source junction list.
        var removeJunctions =
            junction.Zones
            .Where(z => SourceBiomes.Contains(z.Biome))
            .SelectMany(z => z.Junctions)
            .Distinct()
            .ToArray();
        foreach (var removeJunction in removeJunctions)
        {
            sourceJunctions.Remove(removeJunction);
        }
        // While the junction does not meet a target junction...
        do
        {
            // Other rivers may stop when they reach this river.
            targetJunctions.Add(junction);
            // Find the viable roads to flow down.
            var roads = junction.Roads.Where(r => AllowedRoadQualities.Contains(r.Quality)).ToList();
            roads.Sort((x, y) => altitudeComparer.Compare(x.Other(junction), y.Other(junction)));
            // Follow the road with the lowest altitude that isn't a source junction.
            var road = roads.First(r => !sourceJunctions.Contains(r.Other(junction)));
            // Lay the river.
            road.Quality = LaidRoadQuality;
            road.DebugColor = Color.Blue;
            // Move to the next junction.
            junction = road.Other(junction);
        }   // If this is a target junction, stop.
        while (!targetJunctions.Contains(junction));
    }

    /// <summary>
    /// Compares the altitudes of junctions.
    /// </summary>
    /// <param name="altitudes">The altitude map.</param>
    private class Comparer(Dictionary<Junction, int> altitudes) : IComparer<Junction>
    {
        /// <summary>
        /// Compares two junctions by altitude.
        /// </summary>
        /// <param name="x">The first junction.</param>
        /// <param name="y">The second junction.</param>
        public int Compare(Junction? x, Junction? y)
        {
            if (x is null)
                return y is null ? 0 : -1;
            else if (y is null)
                return 1;
            else
                return altitudes[x].CompareTo(altitudes[y]);
        }
    }
}
