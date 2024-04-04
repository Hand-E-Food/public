using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Creates base implementations of base mapping objects.
/// </summary>
public class DefaultMappingObjectFactory : IMappingObjectFactory
{
    private int junctionCount = 0;
    private int zoneCount = 0;

    public Junction CreateJunction(Point location, string? name = null) => new(location, name ?? NextJunctionName());

    public Road CreateRoad(Junction j1, Junction j2, string? name = null) => new(j1, j2, name ?? $"{j1.Name} - {j2.Name}");

    public Zone CreateZone(IEnumerable<Road> roads, string? name = null) => new(roads, name ?? NextZoneName());

    /// <summary>
    /// Gets the next junction's default name.
    /// </summary>
    /// <returns>The next junction's name.</returns>
    private string NextJunctionName() => (++junctionCount).ToString();

    /// <summary>
    /// Gets the next zone's default name.
    /// </summary>
    /// <returns>The next zone's name.</returns>
    private string NextZoneName()
    {
        int value = ++zoneCount;
        string name = string.Empty;
        do
        {
            name = char.ConvertFromUtf32((value % 26) + 65) + name;
            value /= 26;
        }
        while (value > 0);
        return name;
    }
}
