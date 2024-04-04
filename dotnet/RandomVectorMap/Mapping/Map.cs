using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Mapping;

/// <summary>
/// Represents a whole map.
/// </summary>
public class Map
{
    /// <summary>
    /// Returns an enumeration of all zones, including Outside.
    /// </summary>
    /// <value>An enumeration of all zones, including Outside.</value>
    public IEnumerable<Zone> AllZones
    {
        get
        {
            if (Outside is null)
                return Zones;
            else
                return new[] { Outside }.Concat(Zones);
        }
    }

    /// <summary>
    /// Gets the collection of junctions on this map.
    /// </summary>
    /// <value>A collection of junctions.</value>
    public List<Junction> Junctions { get; } = [];

    /// <summary>
    /// Gets the map's exterior zone.
    /// </summary>
    /// <value>The exterior zone.</value>
    public Zone? Outside { get; private set; }

    /// <summary>
    /// Gets a collection of all roads on the map.
    /// </summary>
    /// <value>A collection of roads.</value>
    public List<Road> Roads { get; } = [];

    /// <summary>
    /// Gets a collection of all zones on the map.
    /// </summary>
    /// <value>A collection of zones.</value>
    public List<Zone> Zones { get; } = [];

    /// <summary>
    /// Resets the debug colour of all junctions roads and zones.
    /// </summary>
    public void ClearDebug()
    {
        foreach (var junction in Junctions)
            junction.DebugColor = Color.Transparent;
        foreach (var road in Roads)
            road.DebugColor = Color.Transparent;
        foreach (var zone in Zones)
            zone.DebugColor = Color.Transparent;
    }

    /// <summary>
    /// Initialises the Outside zone by using all roads lacking two assigned zones.
    /// </summary>
    [MemberNotNull(nameof(Outside))]
    public void InitializeOutside()
    {
        if (Outside is not null) throw new InvalidOperationException("InitializeOutside may only be called once.");
        Outside = new(Roads.Where(road => road.Zones.Contains(null)), string.Empty);
    }
}
