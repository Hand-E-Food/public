using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Centres the map.
/// </summary>
public class MapCentralizer : SingleStepMapGeneratorComponent
{
    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        int x = Map.Junctions.Min(j => j.Location.X);
        int y = Map.Junctions.Min(j => j.Location.Y);
        int width = Map.Junctions.Max(j => j.Location.X) - x;
        int height = Map.Junctions.Max(j => j.Location.Y) - y;
        Rectangle bounds = new(x, y, width, height);

        Size offset = new(-width / 2 - x, -height / 2 - y);
        foreach (var junction in Map.Junctions)
            junction.Location += offset;

        base.Step();
    }
}
