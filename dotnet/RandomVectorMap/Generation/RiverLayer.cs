using System;
using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;
using System.Drawing;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Lays rivers between biomes.
    /// </summary>
    public partial class RiverLayer : RoadLayer
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RiverLayer"/> class.
        /// </summary>
        public RiverLayer()
        {
            SourceBiomes = new HashSet<Biome>();
            TargetBiomes = new HashSet<Biome>();
        }

        #region Properties ...

        /// <summary>
        /// The object used to compare altitudes.
        /// </summary>
        private Comparer AltitudeComparer { get; set; }

        /// <summary>
        /// A dictionary of the altitude of each junction.
        /// </summary>
        /// <value>A dictionary of the altitude of each junction.</value>
        private Dictionary<Junction, int> Altitudes { get; set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && SourceJunctions.Count <= 0; } }

        /// <summary>
        /// Gets a collection of the biomes where rivers can start.
        /// </summary>
        /// <value>A collection of biomes.</value>
        public HashSet<Biome> SourceBiomes { get; private set; }

        /// <summary>
        /// The list of viable source junctions.
        /// </summary>
        private SortedSet<Junction> SourceJunctions { get; set; }

        /// <summary>
        /// Gets a collection of the biomes where rivers can end.
        /// </summary>
        /// <value>A collection of biomes.</value>
        public HashSet<Biome> TargetBiomes { get; private set; }

        /// <summary>
        /// The list of viable target junctions.
        /// </summary>
        private HashSet<Junction> TargetJunctions { get; set; }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Set the altitude of each junction.
            var altitudeMapper = new AltitudeMapper(Map);
            altitudeMapper.CalculateAltitudes();
            Altitudes = altitudeMapper.Altitudes;
            AltitudeComparer = new Comparer(Altitudes);

            // Find source junctions.
            var sourceJunctions =
                Map.Zones                                                   // Find all zones
                .Where((z) => SourceBiomes.Contains(z.Biome))               // that have one of the source biomes,
                .SelectMany((z) => z.Junctions)                             // return all junctions adjacent to the zone
                .Distinct();                                                // but only once each.
            SourceJunctions = new SortedSet<Junction>(sourceJunctions, AltitudeComparer);

            // Find taret junctions.
            var targetJunctions =
                Map.AllZones                                                // Find all zones
                .Where((z) => TargetBiomes.Contains(z.Biome))               // that have one of the target biomes,
                .SelectMany((z) => z.Junctions)                             // return all junctions adjacent to the zone
                .Distinct();                                                // but only once each.
            TargetJunctions = new HashSet<Junction>(targetJunctions);
        }

        /// <summary>
        /// Randomly selects a junction from which to start the river.
        /// </summary>
        /// <returns>The junction at which to start the river.  Null if no viable candidate was found.</returns>
        private Junction SelectSourceJunction()
        {
            // Select the lowest point to start the river.
            var result = SourceJunctions.First();
            // Show debug information.
            foreach (var junction in SourceJunctions)
            {
                junction.DebugColor = Color.Red;
            }
            result.DebugColor = Color.Blue;
            // Return the selected source junction.
            return result;
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Select a source junction.
            var junction = SelectSourceJunction();
            // Remove all junctions of all surrounding zones from the source junction list.
            var removeJunctions =
                junction.Zones
                .Where((z) => SourceBiomes.Contains(z.Biome))
                .SelectMany((z) => z.Junctions)
                .Distinct()
                .ToArray();
            foreach (var removeJunction in removeJunctions)
            {
                SourceJunctions.Remove(removeJunction);
            }
            // While the junction does not meet a target junction...
            do
            {
                // Other rivers may stop when they reach this river.
                TargetJunctions.Add(junction);
                // Find the viable roads to flow down.
                var roads = junction.Roads.Where((r) => AllowedRoadQualities.Contains(r.Quality)).ToList();
                roads.Sort((x,y) => AltitudeComparer.Compare(x.Other(junction), y.Other(junction)));
                // Follow the road with the lowest altitude that isn't a source junction.
                var road = roads.First((r) => !SourceJunctions.Contains(r.Other(junction)));
                // Lay the river.
                road.Quality = LaidRoadQuality;
                road.DebugColor = Color.Blue;
                // Move to the next junction.
                junction = road.Other(junction);
            }   // If this is a target junction, stop.
            while (!TargetJunctions.Contains(junction));
        }

        /// <summary>
        /// Compares the altitudes of junctions.
        /// </summary>
        private class Comparer : IComparer<Junction>
        {

            /// <summary>
            /// Initialises a new instance of the <see cref="Comparer"/> class.
            /// </summary>
            /// <param name="altitudes">The altitude map.</param>
            public Comparer(Dictionary<Junction, int> altitudes)
            {
                Altitudes = altitudes;
            }

            /// <summary>
            /// A dictionary of the altitude of each junction.
            /// </summary>
            /// <value>A dictionary of the altitude of each junction.</value>
            private Dictionary<Junction, int> Altitudes { get; set; }

            /// <summary>
            /// Compares two junctions by altitude.
            /// </summary>
            /// <param name="x">The first junction.</param>
            /// <param name="y">The second junction.</param>
            /// <returns></returns>
            public int Compare(Junction x, Junction y)
            {
                return Altitudes[x].CompareTo(Altitudes[y]);
            }
        }
    }
}
