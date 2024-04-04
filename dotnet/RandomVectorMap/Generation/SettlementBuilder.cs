using RandomVectorMap.Mapping;
using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Generation;

/// <summary>
/// Builds cities.
/// </summary>
public class SettlementBuilder : MapGeneratorComponent
{

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && (junctions!.Count == 0 || Map.Junctions.Count(junction => junction.Size != SettlementSize.Undefined) >= MaximumCities);

    /// <summary>
    /// The junctions available to be used as a city.
    /// </summary>
    private List<Junction>? junctions = null;

    /// <summary>
    /// Gets or sets the maximum number of cities.
    /// </summary>
    public int MaximumCities { get; set; } = int.MaxValue;

    /// <summary>
    /// Gets or sets the minimnum distance between cities.
    /// </summary>
    public double MinimumDistance { get; set; } = 0;

    /// <summary>
    /// Gets the list of names that can be used for cities.
    /// </summary>
    public List<string> Names { get; } = [];

    /// <summary>
    /// Gets a dictionary of settlement sizes and the biomes they can spread to.
    /// </summary>
    public Dictionary<SettlementSize, Biome[]> SettleableBiomes { get; } = [];

    /// <summary>
    /// The list of settlement sizes and how often they occur.
    /// </summary>
    public WeightedRandomSet<SettlementSize> SettlementSizeWeights { get; } = [];

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    [MemberNotNull(nameof(junctions))]
    public override void Initialize()
    {
        base.Initialize();

        // Get a list of all unassigned junctions.
        junctions = Map.Junctions.Where(j => j.Size == SettlementSize.Undefined).ToList();
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        if (junctions is null) throw new InvalidOperationException("Class is not initialized.");
        // Build the settlement.
        var junction = Random.Next(junctions);
        junction.Size = Random.Next(SettlementSizeWeights);
        junction.DebugColor = Color.Red;
        if (SettleableBiomes.TryGetValue(junction.Size, out var affectedBiomes))
        {
            foreach (var zone in junction.Zones)
            {
                if (affectedBiomes.Contains(zone.Biome) && zone.SettlementSize < junction.Size)
                {
                    zone.SettlementSize = junction.Size;
                    zone.DebugColor = Color.Red;
                }
            }
        }

        // Remove all nearby junctions from the candidate list.
        junctions.RemoveAll(j => new Vector(j, junction).Length < MinimumDistance);
    }
}
