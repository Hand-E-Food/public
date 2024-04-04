using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Spreads defined biomes over the map.
/// </summary>
public class BiomeSpreader : MapGeneratorComponent
{
    /// <summary>
    /// Gets a collections of biomes to spread.
    /// </summary>
    /// <value>A collection of biomes to spread.</value>
    public HashSet<Biome> Biomes { get; } = [];

    /// <summary>
    /// The weight of each biome currently on the map.
    /// </summary>
    /// <value>A weighted table of biomes.</value>
    private readonly WeightedRandomSet<Biome> biomeWeights = [];

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && biomeWeights.TotalWeight == 0.0;

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        // Calculate the total area of each biome.
        foreach (var zone in Map.Zones.Where(z => Biomes.Contains(z.Biome)))
            biomeWeights[zone.Biome] += zone.Area;
        // Invert the weight of each biome.  Larger biomes are selected less often.
        foreach (var biome in biomeWeights.Values.ToArray())
            biomeWeights[biome] = 1 / biomeWeights[biome];
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // Randomly select a biome to expand.
        var biome = Random.Next(biomeWeights);
        // Find the available zones the biome can spread to.
        var zones =
            Map.Zones                                                           // Find zones
            .Where(zone => zone.Biome == biome)                                 // of the selected biome,
            .SelectMany(zone => zone.Roads.Select(road => road.Other(zone)))    // select their neighbours
            .AssertNotNull()
            .Where(adjacentZone => adjacentZone.Biome == Biome.Undefined)       // that have an undefined biome,
            .ToArray();                                                         // and return the zones as a new array.
        if (zones.Length == 0)
        {   // If there are no available zones...
            // Remove this biome as a candidate.
            biomeWeights[biome] = 0;
            return;
        }
        // Randomly select an available zone.
        var zone = Random.Next(zones);
        // Spead the biome to the new zone.
        zone.Biome = biome;
        // Add the zone's area to the biome's weight.
        biomeWeights[biome] = 1 / (1 / biomeWeights[biome] + zone.Area);
    }
}
