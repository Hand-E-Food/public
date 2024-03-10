using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System;

namespace RandomVectorMap.Mapping
{

    /// <summary>
    /// Represents a whole map.
    /// </summary>
    public class Map
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        {
            Junctions = new List<Junction>();
            Roads = new List<Road>();
            Zones = new List<Zone>();
        }

        #region Properties ...
  
        /// <summary>
        /// Returns an enumeration of all zones, including Outside.
        /// </summary>
        /// <value>An enumeration of all zones, including Outside.</value>
        public IEnumerable<Zone> AllZones
        {
            get 
            {
                if (Outside == null)
                    return Zones;
                else
                    return new[] { Outside }.Concat(Zones); 
            }
        }

        /// <summary>
        /// Gets the collection of junctions on this map.
        /// </summary>
        /// <value>A collection of junctions.</value>
        public List<Junction> Junctions { get; private set; }

        /// <summary>
        /// Gets the map's exterior zone.
        /// </summary>
        /// <value>The exterior zone.</value>
        public Zone Outside { get; private set; }

        /// <summary>
        /// Gets a collection of all roads on the map.
        /// </summary>
        /// <value>A collection of roads.</value>
        public List<Road> Roads { get; private set; }

        /// <summary>
        /// Gets a collection of all zones on the map.
        /// </summary>
        /// <value>A collection of zones.</value>
        public List<Zone> Zones { get; private set; }
 
        #endregion

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
        public void InitializeOutside()
        {
            // Validate date.
            if (Outside != null) throw new InvalidOperationException("InitializeOutside may only be called once.");
            // Create the outside zone.
            Outside = new Zone(Roads.Where((r) => r.Zones.Contains(null)));
        }
    }
}
