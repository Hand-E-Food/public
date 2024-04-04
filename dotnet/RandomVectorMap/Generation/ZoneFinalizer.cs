using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Removes all undefined zones from the map.
/// </summary>
public class ZoneFinalizer : SingleStepMapGeneratorComponent
{
    /// <summary>
    /// Get a collection of biomes that undefined zones may assume.
    /// </summary>
    /// <value>A list of biomes that may be assumed.</value>
    public List<Biome> AllowedBiomes { get; } = [];

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // Set all undefined zones to match one of their neighbours.
        foreach (var zone in Map.Zones.Where(zone => zone.Biome == Biome.Undefined))
        {
            var biomes =
                zone.Junctions
                .SelectMany(junction => junction.Zones)
                .Distinct()
                .Select(z => z.Biome)
                .Where(AllowedBiomes.Contains)
                .ToArray();
            if (biomes.Length > 0)
            {
                zone.Biome = Random.Next(biomes);
                zone.DebugColor = Color.Blue;
            }
            else
            {
                zone.Biome = Random.Next(AllowedBiomes);
                zone.DebugColor = Color.Red;
            }
        }
        base.Step();
    }
}
