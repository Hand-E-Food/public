using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Seeds biomes on the map.
/// </summary>
public class BiomeSeeder : MapGeneratorComponent
{
    /// <summary>
    /// Gets the collection of biomes to seed.  Duplicate values are expected.
    /// </summary>
    /// <value>A collection of biomes to seed.</value>
    public List<Biome> Biomes { get; } = [];

    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && Biomes.Count <= 0;

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // Find the undefined zones.
        var zones = Map.Zones.Where(zone => zone.Biome == Biome.Undefined).ToArray();
        if (zones.Length > 0)
        {   // If there are more undefined zones...
            // Set the biome of a random zone.
            Random.Next(zones).Biome = Random.Pop(Biomes);
        }
        else
        {   // If there are no more undefined zones...
            // This task is finished.
            Biomes.Clear();
        }
    }
}
