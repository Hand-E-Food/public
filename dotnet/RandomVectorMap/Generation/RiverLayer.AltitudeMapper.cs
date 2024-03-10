using System;
using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{
    partial class RiverLayer
    {

        /// <summary>
        /// Calculates the altitude of every junction on the map.
        /// </summary>
        private class AltitudeMapper
        {

            /// <summary>
            /// Initialises a new instance of the <see cref="RandomVectorMap.Generation.RiverLayer"/> class.
            /// </summary>
            /// <param name="map">The map to examine.</param>
            public AltitudeMapper(Map map)
            {
                this.Map = map;
                this.Altitudes = new Dictionary<Junction, int>();
                foreach (var junction in Map.Junctions)
                {
                    this.Altitudes.Add(junction, int.MaxValue);
                }
                this.Queue = new Queue<Junction>();

                BiomeAltitudes = new Dictionary<Biome, int>();
                BiomeAltitudes.Add(Biome.Ocean    ,     0);
                BiomeAltitudes.Add(Biome.Lake     ,     0);
                BiomeAltitudes.Add(Biome.Swamp    ,     1);
                BiomeAltitudes.Add(Biome.Pasture  ,    10);
                BiomeAltitudes.Add(Biome.Forest   ,    10);
                BiomeAltitudes.Add(Biome.Desert   ,   100);
                BiomeAltitudes.Add(Biome.Undefined,   100);
                BiomeAltitudes.Add(Biome.Mountain ,  1000);
                BiomeAltitudes.Add(Biome.Snow     , 10000);
            }

            #region Properties ...

            /// <summary>
            /// Gets the dictionary of the altitude as each junction on the map.
            /// </summary>
            /// <value>A dictionary of the altitude at each junction on the map.</value>
            public Dictionary<Junction, int> Altitudes { get; private set; }

            /// <summary>
            /// The relative altitudes of each biome.
            /// </summary>
            private Dictionary<Biome, int> BiomeAltitudes { get; set; }

            /// <summary>
            /// Gets the map being examined.
            /// </summary>
            /// <value>The map being examined.</value>
            public Map Map { get; private set; }

            /// <summary>
            /// The queue of junctions to process.
            /// </summary>
            private Queue<Junction> Queue { get; set; }

            #endregion

            /// <summary>
            /// Calculates the altitude of each junction on the map.
            /// </summary>
            public void CalculateAltitudes()
            {
                EnqueueSeaLevel();
                while (Queue.Count > 0)
                {
                    var junction = Queue.Dequeue();
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
                int targetAltitude = Altitudes[sourceJunction] + road.Zones.Sum((z) => BiomeAltitudes[z.Biome]);
                // Check if this altitude is lower than the recorded altitude.
                if (Altitudes[targetJuction] > targetAltitude)
                {   // If this altitude is lower that the recorded altitude...
                    // Set the new altitude.
                    Altitudes[targetJuction] = targetAltitude;
                    // Calculate altitudes extending from the target junction.
                    if (!Queue.Contains(targetJuction)) Queue.Enqueue(targetJuction);
                }
            }

            /// <summary>
            /// Initialises the queue to start calculating altitudes at sea level.
            /// </summary>
            private void EnqueueSeaLevel()
            {
                foreach (var junction in Map.Outside.Junctions)
                {
                    Altitudes[junction] = 0;
                    Queue.Enqueue(junction);
                }
            }
        }
    }
}