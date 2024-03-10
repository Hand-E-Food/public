using System.Collections.Generic;
using System.Drawing;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Creates specific implementations of base mapping objects.
    /// </summary>
    public interface IMappingObjectFactory
    {

        /// <summary>
        /// Creates a new junction.
        /// </summary>
        /// <param name="Location">The junction's location.</param>
        /// <returns>An initialised instance of a Junction object.</returns>
        Junction CreateJunction(Point location);

        /// <summary>
        /// Creates a new road.
        /// </summary>
        /// <param name="j1">The first junction endpoint.</param>
        /// <param name="j2">The second junction endpoint.</param>
        /// <returns>An initialised instance of a Road object.</returns>
        Road CreateRoad(Junction j1, Junction j2);

        /// <summary>
        /// Creates a new Zone.
        /// </summary>
        /// <param name="roads">The zone's bordering roads.  The roads must form a closed loop but may be in
        /// any order.</param>
        /// <returns>An initialised instance of a Zone object.</returns>
        Zone CreateZone(IEnumerable<Road> roads);
    }
}
