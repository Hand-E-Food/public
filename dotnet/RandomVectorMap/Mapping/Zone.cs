using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Mapping;

/// <summary>
/// Represents a zone surrounded by roads.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{Name}")]
public class Zone
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Zone"/> class.
    /// </summary>
    /// <param name="roads">The roads surrounding this zone. The roads must form a closed loop.
    /// </param>
    /// <param name="name">This zone's name.</param>
    /// <exception cref="ArgumentNullException">roads is null.</exception>
    public Zone(IEnumerable<Road> roads, string name)
    {
        ArgumentNullException.ThrowIfNull(roads);

        Name = name;
        this.roads = roads.ToArray();
        foreach (var road in this.roads)
            road.Replace(null, this);

        SortRoads();
        Area = MeasureArea();
    }

    /// <summary>
    /// Gets this zone's area.
    /// </summary>
    /// <value>This zone's area.</value>
    public double Area { get; private set; }

    /// <summary>
    /// Gets or sets the zone's biome.
    /// </summary>
    /// <value>A biome.</value>
    public Biome Biome { get; set; } = Biome.Undefined;

    /// <summary>
    /// Gets or sets the colour to paint this zone.
    /// </summary>
    public Color DebugColor { get; set; } = Color.Transparent;

    /// <summary>
    /// Gets this regions adjacent junctions.
    /// </summary>
    /// <value>A collection of this regions adjacent junctions.</value>
    /// <remarks>This value is only set after CleanUp() is called.</remarks>
    public IEnumerable<Junction> Junctions => junctions;
    private Junction[] junctions;

    /// <summary>
    /// Gets or sets this zone's name.
    /// </summary>
    /// <value>This zone's name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets a collection of the surrounding roads.
    /// </summary>
    /// <value>A collection of roads.</value>
    public IEnumerable<Road> Roads => roads;
    private Road[] roads;

    /// <summary>
    /// Gets or sets the size of this zone's settlement.
    /// </summary>
    public SettlementSize SettlementSize { get; set; } = SettlementSize.Undefined;

    /// <summary>
    /// Measures the area of this zone.
    /// </summary>
    /// <returns>Ths zone's area.</returns>
    private double MeasureArea()
    {
        if (roads.Length == 3)
        {   // If the roads form a triangle...
            return MeasureTriangleArea(Roads.Select(r => r.Line).ToArray());
        }
        else
        {
            //TODO: Implement polygon area calculation.
            return 0;
        }
    }

    /// <summary>
    /// Measures the area of a triangle given three lines.
    /// </summary>
    /// <param name="lines">The lines of the triangle.</param>
    /// <returns>The triangle's area.</returns>
    private static double MeasureTriangleArea(params Line[] lines)
    {
        ArgumentNullException.ThrowIfNull(lines);
        if (lines.Length != 3) throw new ArgumentException("Exactly 3 lines must be specified.");
        // Use Heron's Formula: http://www.mathopenref.com/heronsformula.html
        double p = lines.Sum(line => line.Length) / 2;
        return Math.Sqrt(p * lines.Select(line => p - line.Length).Aggregate((old, add) => old * add));
    }

    /// <summary>
    /// Sorts the roads in cyclic order.
    /// </summary>
    /// <returns>True if the roads form a closed loop; otherwise, false.</returns>
    [MemberNotNull(nameof(junctions))]
    private void SortRoads()
    {
        // Initialise the replacement list.
        List<Road> newRoads = new(roads.Length);
        List<Junction> newJunctions = new(roads.Length);

        // Sort this zone's roads in cyclic order.
        Junction firstJunction = roads[0].Junctions.First();
        Junction junction = firstJunction;
        Road? road = null;
        do
        {
            newJunctions.Add(junction);
            road = junction.Roads.FirstOrDefault(r => r.Zones.Contains(this) && r != road);
            if (road is null) throw new ArgumentException("The roads must form a closed loop.");
            newRoads.Add(road);
            junction = road.Other(junction);
        }
        while (junction != firstJunction);

        // Validate that all roads were used.
        if (newRoads.Count != roads.Length) throw new ArgumentException("The roads must form a closed loop.");

        // Replace the lists.
        junctions = [.. newJunctions];
        roads = [.. newRoads];
    }
}
