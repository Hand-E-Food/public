using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Promotes the biome of a zone that meets a specific criteria.
/// </summary>
public class BiomePromoter : SingleStepMapGeneratorComponent
{
    /// <summary>
    /// Gets or sets the biome to set in zones that meet the condition.
    /// </summary>
    /// <value>The biome to set.</value>
    public Biome Biome { get; set; } = Biome.Undefined;

    /// <summary>
    /// Gets or sets the condition that a zone must meet to have its biome reassigned.
    /// </summary>
    /// <value>The condition a zone must meet.</value>
    public Predicate<Zone> Condition { get; set; } = _ => false;

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        // For each zone that meets the conditon...
        foreach (var zone in Map.Zones)
        {
            // If the zone meets the condition, replace its biome.
            if (Condition(zone))
            {
                zone.Biome = Biome;
                zone.DebugColor = Color.Red;
            }
        }
        // This task is finished.
        base.Step();
    }
}
