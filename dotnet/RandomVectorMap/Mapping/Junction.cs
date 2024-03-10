using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RandomVectorMap.Mapping
{

    /// <summary>
    /// Represents a junction of two or more roads.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class Junction
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="Junction"/> class.
        /// </summary>
        /// <param name="location">The junction's location.</param>
        public Junction(Point location)
        {
            DebugColor = Color.Transparent;
            Location = location;
            Roads = new List<Road>();
            Size = SettlementSize.Undefined;
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the colour to paint this juction.
        /// </summary>
        public Color DebugColor { get; set; }

        /// <summary>
        /// Gets or sets this junction's coordinates.
        /// </summary>
        /// <value>A coordinate.</value>
        public Point Location { get; set; }

        /// <summary>
        /// Gets or sets this junction's name.
        /// </summary>
        /// <value>This junction's name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets a collection of the roads that meet at this junction.
        /// </summary>
        /// <value>A collection of roads.</value>
        public List<Road> Roads { get; private set; }

        /// <summary>
        /// Gets or sets the size of this junction's settlement.
        /// </summary>
        public SettlementSize Size { get; set; }

        /// <summary>
        /// Gets a collection of all zones bordering this junction.
        /// </summary>
        /// <value>A collection of zones.</value>
        public IEnumerable<Zone> Zones { get { return Roads.SelectMany((r) => r.Zones.Where((z) => z != null)).Distinct(); } }

        #endregion

        /// <summary>
        /// Returns the junction's location.
        /// </summary>
        /// <param name="j">The junction to convert.</param>
        /// <returns>The junciton's location.</returns>
        public static implicit operator Point(Junction j)
        {
            if (j == null)
                return Point.Empty;
            else
                return j.Location;
        }
    }
}
