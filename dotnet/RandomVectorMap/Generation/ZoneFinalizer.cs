using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Removes all undefined zones from the map.
    /// </summary>
    public class ZoneFinalizer : SingleStepMapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="ZoneFinalizer"/> class.
        /// </summary>
        public ZoneFinalizer()
        {
            AllowedBiomes = new List<Biome>();
        }

        #region Properties ...

        /// <summary>
        /// Get a collection of biomes that undefined zones may assume.
        /// </summary>
        /// <value>A list of biomes that may be assumed.</value>
        public List<Biome> AllowedBiomes { get; private set; }

        #endregion

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Set all undefined zones to match one of their neighbours.
            foreach (var zone in Map.Zones.Where((z) => z.Biome == Biome.Undefined))
            {
                var biomes =
                    zone.Junctions
                    .SelectMany((j) => j.Zones)
                    .Distinct()
                    .Select((z) => z.Biome)
                    .Where((b) => AllowedBiomes.Contains(b))
                    .ToArray();
                if (biomes.Length > 0)
                {
                    zone.Biome = biomes[Random.Next(biomes.Length)];
                    zone.DebugColor = Color.Blue;
                }
                else
                {
                    zone.Biome = AllowedBiomes[Random.Next(AllowedBiomes.Count)];
                    zone.DebugColor = Color.Red;
                }
            }
            base.Step();
        }
    }
}
