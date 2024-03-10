using System;
using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Spreads defined biomes over the map.
    /// </summary>
    public class BiomeSpreader : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="BiomeSpreader"/> class.
        /// </summary>
        public BiomeSpreader()
        {
            Biomes = new HashSet<Biome>();
            BiomeWeights = new WeightedRandomSet<Biome>();
        }

        #region Properties ...

        /// <summary>
        /// Gets a collections of biomes to spread.
        /// </summary>
        /// <value>A collection of biomes to spread.</value>
        public HashSet<Biome> Biomes { get; private set; }

        /// <summary>
        /// The weight of each biome currently on the map.
        /// </summary>
        /// <value>A weighted table of biomes.</value>
        private WeightedRandomSet<Biome> BiomeWeights { get; set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && BiomeWeights.TotalWeight == 0.0; } }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Calculate the total area of each biome.
            foreach (var zone in Map.Zones.Where((z) => Biomes.Contains(z.Biome)))
            {
                BiomeWeights[zone.Biome] += zone.Area;
            }
            // Invert the weight of each biome.  Larger biomes are selected less often.
            foreach (var biome in BiomeWeights.Values.ToArray())
            {
                BiomeWeights[biome] = 1 / BiomeWeights[biome];
            }
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Randomly select a biome to expand.
            var biome = BiomeWeights.Select();
            // Find the available zones the biome can spread to.
            var zones =
                Map.Zones                                                   // Find zones
                .Where((z) => z.Biome == biome)                             // of the selected biome,
                .SelectMany((z) => z.Roads.Select((r) => r.Other(z)))       // select their neighbours
                .Where((adjacentZ) => adjacentZ.Biome == Biome.Undefined)   // that have an undefined biome,
                .ToArray();                                                 // and return the zones as a new array.
            if (zones.Length == 0)
            {   // If there are no available zones...
                // Remove this biome as a candidate.
                BiomeWeights[biome] = 0;
                return;
            }
            // Randomly select an available zone.
            var zone = zones[Random.Next(zones.Length)];
            // Spead the biome to the new zone.
            zone.Biome = biome;
            // Add the zone's area to the biome's weight.
            BiomeWeights[biome] = 1 / (1 / BiomeWeights[biome] + zone.Area);
        }
    }
}
