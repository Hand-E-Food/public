using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Collections.ObjectModel;

namespace RandomVectorMap.Mapping
{

    /// <summary>
    /// Represents a zone surrounded by roads.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class Zone
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="Zone"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">roads is null.</exception>
        public Zone(IEnumerable<Road> roads)
        {
            if (roads == null) throw new ArgumentNullException("roads");

            Biome = Biome.Undefined;
            DebugColor = Color.Transparent;
            _Roads = roads.ToArray();
            SettlementSize = Mapping.SettlementSize.Undefined;

            foreach (var road in _Roads)
            {
                road.Replace(null, this);
            }

            SortRoads();
            Area = MeasureArea();
        }

        #region Properties ...

        /// <summary>
        /// Gets this zone's area.
        /// </summary>
        /// <value>This zone's area.</value>
        public double Area { get; private set; }

        /// <summary>
        /// Gets or sets the zone's biome.
        /// </summary>
        /// <value>A biome.</value>
        public Biome Biome { get; set; }

        /// <summary>
        /// Gets or sets the colour to paint this zone.
        /// </summary>
        public Color DebugColor { get; set; }

        /// <summary>
        /// Gets this regions adjacent junctions.
        /// </summary>
        /// <value>A collection of this regions adjacent junctions.</value>
        /// <remarks>This value is only set after CleanUp() is called.</remarks>
        public IEnumerable<Junction> Junctions { get { return _Junctions; } }
        private Junction[] _Junctions;

        /// <summary>
        /// Gets or sets this zone's name.
        /// </summary>
        /// <value>This zone's name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets a collection of the surrounding roads.
        /// </summary>
        /// <value>A collection of roads.</value>
        public IEnumerable<Road> Roads { get { return _Roads; } }
        private Road[] _Roads;

        /// <summary>
        /// Gets or sets the size of this zone's settlement.
        /// </summary>
        public SettlementSize SettlementSize { get; set; }

        #endregion

        /// <summary>
        /// Measures the area of this zone.
        /// </summary>
        /// <returns>Ths zone's area.</returns>
        private double MeasureArea()
        {
            if (_Roads.Length == 3)
            {   // If the roads form a triangle...
                return MeasureTriangleArea(Roads.Select((r) => r.Line).ToArray());
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
        private double MeasureTriangleArea(params Line[] lines)
        {
            if (lines == null) throw new ArgumentNullException("lines");
            if (lines.Length != 3) throw new ArgumentException("Exactly 3 lines must be specified.");
            // Use Heron's Formula: http://www.mathopenref.com/heronsformula.html
            double p = lines.Sum((l) => l.Length) / 2;
            return Math.Sqrt(p * lines.Select((l) => p - l.Length).Aggregate((x, y) => x * y));
        }

        /// <summary>
        /// Sorts the roads in cyclic order.
        /// </summary>
        /// <returns>True if the roads form a closed loop; otherwise, false.</returns>
        private void SortRoads()
        {
            // Initialise the replacement list.
            List<Road> newRoads = new List<Road>(_Roads.Length);
            List<Junction> newJunctions = new List<Junction>(_Roads.Length);

            // Sort this zone's roads in cyclic order.
            Junction firstJunction = _Roads[0].Junctions.First();
            Junction junction = firstJunction;
            Road road = null;
            do
            {
                newJunctions.Add(junction);
                road = junction.Roads.Where((r) => r.Zones.Contains(this) && r != road).FirstOrDefault();
                if (road == null) throw new ArgumentException("The roads must form a closed loop.");
                newRoads.Add(road);
                junction = road.Other(junction);
            }
            while (junction != firstJunction);

            // Validate that all roads were used.
            if (newRoads.Count != _Roads.Length) throw new ArgumentException("The roads must form a closed loop.");

            // Replace the lists.
            _Junctions = newJunctions.ToArray();
            _Roads = newRoads.ToArray();
        }
    }
}
