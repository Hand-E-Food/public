using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

partial class RiverLayer
{
    /// <summary>
    /// Calculates the altitude of every junction on the map.
    /// </summary>
    private class AltitudeMapper
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="RiverLayer"/> class.
        /// </summary>
        /// <param name="map">The map to examine.</param>
        public AltitudeMapper(Map map)
        {
            Map = map;
            foreach (var junction in Map.Junctions)
                Altitudes.Add(junction, int.MaxValue);
        }

        /// <summary>
        /// Gets the dictionary of the altitude as each junction on the map.
        /// </summary>
        /// <value>A dictionary of the altitude at each junction on the map.</value>
        public Dictionary<Junction, int> Altitudes { get; } = [];

        /// <summary>
        /// The relative altitudes of each biome.
        /// </summary>
        private static readonly Dictionary<Biome, int> BiomeAltitudes = new()
        {
            { Biome.Ocean, 0 },
            { Biome.Lake, 0 },
            { Biome.Swamp, 1 },
            { Biome.Pasture, 10 },
            { Biome.Forest, 10 },
            { Biome.Desert, 100 },
            { Biome.Undefined, 100 },
            { Biome.Mountain, 1000 },
            { Biome.Snow, 10000 }
        };

        /// <summary>
        /// Gets the map being examined.
        /// </summary>
        /// <value>The map being examined.</value>
        public Map Map { get; }

        /// <summary>
        /// The queue of junctions to process.
        /// </summary>
        private readonly Queue<Junction> queue = [];

        /// <summary>
        /// Calculates the altitude of each junction on the map.
        /// </summary>
        public void CalculateAltitudes()
        {
            EnqueueSeaLevel();
            while (queue.Count > 0)
            {
                var junction = queue.Dequeue();
                CalculateAltitudesFrom(junction);
            }
        }

        /// <summary>
        /// Calculates the altitude of the junctions adjacent to the specified junction.
        /// </summary>
        /// <param name="junction">The hub junction.</param>
        private void CalculateAltitudesFrom(Junction junction)
        {
            foreach (var road in junction.Roads)
            {
                CalculateAltitude(junction, road);
            }
        }

        /// <summary>
        /// Calculates the altitude of the junction at the other end of the specified road.
        /// </summary>
        /// <param name="sourceJunction">The downhill junction.</param>
        /// <param name="road">The road to traverse.</param>
        private void CalculateAltitude(Junction sourceJunction, Road road)
        {
            var targetJuction = road.Other(sourceJunction);
            // Calculate the uphill altitude.
            int targetAltitude = Altitudes[sourceJunction] + road.Zones.AssertNotNull().Sum(zone => BiomeAltitudes[zone.Biome]);
            // Check if this altitude is lower than the recorded altitude.
            if (Altitudes[targetJuction] > targetAltitude)
            {   // If this altitude is lower that the recorded altitude...
                // Set the new altitude.
                Altitudes[targetJuction] = targetAltitude;
                // Calculate altitudes extending from the target junction.
                if (!queue.Contains(targetJuction)) queue.Enqueue(targetJuction);
            }
        }

        /// <summary>
        /// Initialises the queue to start calculating altitudes at sea level.
        /// </summary>
        private void EnqueueSeaLevel()
        {
            if (Map.Outside is null) throw new InvalidOperationException($"The {nameof(Map)}'s {nameof(Map.Outside)} {nameof(Zone)} is not defined.");
            foreach (var junction in Map.Outside.Junctions)
            {
                Altitudes[junction] = 0;
                queue.Enqueue(junction);
            }
        }
    }
}