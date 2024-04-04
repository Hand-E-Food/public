namespace RandomVectorMap.Mapping;

/// <summary>
/// Represents a junction of two or more roads.
/// </summary>
/// <param name="location">The junction's location.</param>
[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Junction(Point location, string name)
{
    /// <summary>
    /// Gets or sets the colour to paint this juction.
    /// </summary>
    public Color DebugColor { get; set; } = Color.Transparent;

    /// <summary>
    /// Gets this junction's coordinates.
    /// </summary>
    /// <value>A coordinate.</value>
    public Point Location { get; set; } = location;

    /// <summary>
    /// Gets or sets this junction's name.
    /// </summary>
    /// <value>This junction's name.</value>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets a collection of the roads that meet at this junction.
    /// </summary>
    /// <value>A collection of roads.</value>
    public List<Road> Roads { get; } = [];

    /// <summary>
    /// Gets or sets the size of this junction's settlement.
    /// </summary>
    public SettlementSize Size { get; set; } = SettlementSize.Undefined;

    /// <summary>
    /// Gets a collection of all zones bordering this junction.
    /// </summary>
    /// <value>A collection of zones.</value>
    public IEnumerable<Zone> Zones => Roads.SelectMany(road => road.Zones.WhereNotNull()).Distinct();

    /// <summary>
    /// Returns the junction's location.
    /// </summary>
    /// <param name="junction">The junction to convert.</param>
    /// <returns>The junction's location.</returns>
    public static implicit operator Point(Junction junction) => junction.Location;
}
