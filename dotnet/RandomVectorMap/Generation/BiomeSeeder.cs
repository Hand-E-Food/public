using System;
using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Seeds biomes on the map.
    /// </summary>
    public class BiomeSeeder : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="BiomeSeeder"/> class.
        /// </summary>
        public BiomeSeeder()
        {
            Biomes = new List<Biome>();
        }

        #region Properties ...

        /// <summary>
        /// Gets the collection of biomes to seed.  Duplicate values are expected.
        /// </summary>
        /// <value>A collection of biomes to seed.</value>
        public List<Biome> Biomes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && Biomes.Count <= 0; } }

        #endregion

        /// <summary>
        /// Removes and returns a random biome from the list.
        /// </summary>
        /// <returns>A random biome.</returns>
        private Biome GetRandomBiome()
        {
            int index = Random.Next(Biomes.Count);
            var result = Biomes[index];
            Biomes.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Find the undefined zones.
            var zones = Map.Zones.Where((z) => z.Biome == Biome.Undefined).ToArray();
            if (zones.Length > 0)
            {   // If there are more undefined zones...
                // Set the biome of a random zone.
                var zone = zones[Random.Next(zones.Length)];
                zone.Biome = GetRandomBiome();
            }
            else
            {   // If there are no more undefined zones...
                // This task is finished.
                Biomes.Clear();
            }
        }
    }
}
