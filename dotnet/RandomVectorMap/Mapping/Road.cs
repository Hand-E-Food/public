namespace RandomVectorMap.Mapping;

/// <summary>
/// Represents a road between two junctions.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Road
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Road"/> class.
    /// </summary>
    /// <param name="j1">The road's starting junction.</param>
    /// <param name="j2">The road's ending junction.</param>
    /// <param name="name">The road's name.</param>
    /// <exception cref="ArgumentNullException">One or more parameters are null.</exception>
    public Road(Junction j1, Junction j2, string name)
    {
        junctions = [j1, j2];
        Line = new(j1.Location, j2.Location);
        Name = name;

        j1.Roads.Add(this);
        j2.Roads.Add(this);
    }

    /// <summary>
    /// Gets or sets the colour to paint this road.
    /// </summary>
    public Color DebugColor { get; set; } = Color.Transparent;

    /// <summary>
    /// Gets the pair of junctions at either end of this road.
    /// </summary>
    /// <value>A pair of junctions.</value>
    public IEnumerable<Junction> Junctions => junctions;
    private readonly Junction[] junctions;

    /// <summary>
    /// Gets the straight-line distance between this road's two junctions.
    /// </summary>
    /// <value>The straight-line distance between this road's two junctions.</value>
    public double Length => Line.Length;

    /// <summary>
    /// Gets the line this road follows.
    /// </summary>
    /// <value>A line.</value>
    public Line Line { get; }

    /// <summary>
    /// Gets or sets this road's name.
    /// </summary>
    /// <value>This road's name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the quality of this road.
    /// </summary>
    /// <value>A road quality.</value>
    public RoadQuality Quality { get; set; } = RoadQuality.Undefined;

    /// <summary>
    /// Gets the pair of zones bordering this road.
    /// </summary>
    /// <value>A pair of zones.</value>
    public IEnumerable<Zone?> Zones => zones;
    private readonly Zone?[] zones = [null, null];

    /// <summary>
    /// Returns the junction that isn't the specified junction.
    /// </summary>
    /// <param name="junction">The known junction.</param>
    /// <returns>The junction that isn't the specified junction.</returns>
    public Junction Other(Junction junction) => Junctions.First(j => j != junction);

    /// <summary>
    /// Returns the zone that isn't the specified zone.
    /// </summary>
    /// <param name="zone">The known zone.</param>
    /// <returns>The zone that isn't the specified zone.</returns>
    public Zone? Other(Zone zone) => Zones.First(z => z != zone);

    /// <summary>
    /// Replaces a zone with another zone.
    /// </summary>
    /// <param name="oldZone">The zone to replace.</param>
    /// <param name="newZone">The replacement zone.</param>
    /// <remarks>This method should only be called by a zone.</remarks>
    public void Replace(Zone? oldZone, Zone newZone)
    {
        int i = Array.IndexOf(zones, oldZone);
        if (i < 0) throw new ArgumentException($"The {nameof(oldZone)} was not found.");
        zones[i] = newZone;
    }

    /// <summary>
    /// Converts the road to a line.
    /// </summary>
    /// <param name="road">The road to convert.</param>
    /// <returns>The road's line.</returns>
    public static implicit operator Line(Road road) => road.Line;
}
