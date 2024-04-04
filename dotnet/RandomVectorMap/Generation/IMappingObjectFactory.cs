using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Creates specific implementations of base mapping objects.
/// </summary>
public interface IMappingObjectFactory
{
    /// <summary>
    /// Creates a new junction.
    /// </summary>
    /// <param name="location">The junction's location.</param>
    /// <param name="name">The junction's name. If omitted, a default name is created.</param>
    /// <returns>An initialised instance of a Junction object.</returns>
    Junction CreateJunction(Point location, string? name = null);

    /// <summary>
    /// Creates a new road.
    /// </summary>
    /// <param name="j1">The first junction endpoint.</param>
    /// <param name="j2">The second junction endpoint.</param>
    /// <param name="name">The road's name. If omitted, a default name based on the junction names
    /// is created.</param>
    /// <returns>An initialised instance of a Road object.</returns>
    Road CreateRoad(Junction j1, Junction j2, string? name = null);

    /// <summary>
    /// Creates a new Zone.
    /// </summary>
    /// <param name="roads">The zone's bordering roads.  The roads must form a closed loop but may be in
    /// any order.</param>
    /// <param name="name">The zone's name. If omitted, a default name is created.</param>
    /// <returns>An initialised instance of a Zone object.</returns>
    Zone CreateZone(IEnumerable<Road> roads, string? name = null);
}
