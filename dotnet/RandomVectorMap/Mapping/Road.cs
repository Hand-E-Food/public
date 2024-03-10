using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RandomVectorMap.Mapping
{

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
        /// <exception cref="ArgumentNullException">One or more parameters are null.</exception>
        public Road(Junction j1, Junction j2)
        {
            if (j1 == null) throw new ArgumentNullException("j1");
            if (j2 == null) throw new ArgumentNullException("j2");

            DebugColor = Color.Transparent;
            _Junctions = new[] { j1, j2 };
            Line = new Line(j1.Location, j2.Location);
            Quality = RoadQuality.Undefined;
            _Zones = new Zone[] { null, null };

            j1.Roads.Add(this);
            j2.Roads.Add(this);
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the colour to paint this road.
        /// </summary>
        public Color DebugColor { get; set; }

        /// <summary>
        /// Gets the pair of junctions at either end of this road.
        /// </summary>
        /// <value>A pair of junctions.</value>
        public IEnumerable<Junction> Junctions { get { return _Junctions; } }
        private readonly Junction[] _Junctions;

        /// <summary>
        /// Gets the straight-line distance between this road's two junctions.
        /// </summary>
        /// <value>The straight-line distance between this road's two junctions.</value>
        public double Length
        {
            get { return Line.Length; }
        }

        /// <summary>
        /// Gets the line this road follows.
        /// </summary>
        /// <value>A line.</value>
        public Line Line { get; private set; }

        /// <summary>
        /// Gets or sets this road's name.
        /// </summary>
        /// <value>This road's name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the quality of this road.
        /// </summary>
        /// <value>A road quality.</value>
        public RoadQuality Quality { get; set; }

        /// <summary>
        /// Gets the pair of zones bordering this road.
        /// </summary>
        /// <value>A pair of zones.</value>
        public IEnumerable<Zone> Zones { get { return _Zones; } }
        private readonly Zone[] _Zones;

        #endregion

        /// <summary>
        /// Returns the junction that isn't the specified junction.
        /// </summary>
        /// <param name="junction">The known junction.</param>
        /// <returns>The junction that isn't the specified junction.</returns>
        public Junction Other(Junction junction)
        {
            return Junctions.First((j) => j != junction);
        }
        /// <summary>
        /// Returns the zone that isn't the specified zone.
        /// </summary>
        /// <param name="zone">The known zone.</param>
        /// <returns>The zone that isn't the specified zone.</returns>
        public Zone Other(Zone zone)
        {
            return Zones.First((z) => z != zone);
        }

        /// <summary>
        /// Replaces a zone with another zone.
        /// </summary>
        /// <param name="oldZone">The zone to replace.</param>
        /// <param name="newZone">The replacement zone.</param>
        /// <remarks>This method should only be called by a zone.</remarks>
        public void Replace(Zone oldZone, Zone newZone)
        {
            int i = Array.IndexOf(_Zones, oldZone);
            if (i < 0) throw new ArgumentException("The oldZone was not found.");
            _Zones[i] = newZone;
        }

        /// <summary>
        /// Converts the road to a line.
        /// </summary>
        /// <param name="road">The road to convert.</param>
        /// <returns>The road's line.</returns>
        public static implicit operator Line(Road road)
        {
            return road.Line;
        }
    }
}
