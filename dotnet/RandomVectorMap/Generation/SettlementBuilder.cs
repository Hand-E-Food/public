using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Builds cities.
    /// </summary>
    public class SettlementBuilder : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="SettlementBuilder"/> class.
        /// </summary>
        public SettlementBuilder()
        {
            MaximumCities = int.MaxValue;
            MinimumDistance = 0;
            Names = new List<string>();
            SettleableBiomes = new Dictionary<SettlementSize, Biome[]>();
            SettlementSizeWeights = new WeightedRandomSet<SettlementSize>();
        }

        #region Properties ...

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished 
        { 
            get 
            { 
                return IsInitialized 
                    && (    Junctions.Count == 0
                         || Map.Junctions.Count((j) => j.Size != SettlementSize.Undefined) >= MaximumCities
                       ); 
            }
        }

        /// <summary>
        /// The junctions available to be used as a city.
        /// </summary>
        private List<Junction> Junctions;

        /// <summary>
        /// Gets or sets the maximum number of cities.
        /// </summary>
        public int MaximumCities { get; set; }

        /// <summary>
        /// Gets or sets the minimnum distance between cities.
        /// </summary>
        public double MinimumDistance { get; set; }

        /// <summary>
        /// Gets the list of names that can be used for cities.
        /// </summary>
        public List<string> Names { get; private set; }

        /// <summary>
        /// Gets a dictionary of settlement sizes and the biomes they can spread to.
        /// </summary>
        public Dictionary<SettlementSize, Biome[]> SettleableBiomes { get; private set; }

        /// <summary>
        /// The list of settlement sizes and how often they occur.
        /// </summary>
        public WeightedRandomSet<SettlementSize> SettlementSizeWeights;

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Get a list of all unassigned junctions.
            Junctions = new List<Junction>(Map.Junctions.Where((j) => j.Size == SettlementSize.Undefined));

            // Apply the random number generator to the weighted lists.
            SettlementSizeWeights.Random = Random;
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Build the settlement.
            var junction = Junctions[Random.Next(Junctions.Count)];
            junction.Size = SettlementSizeWeights.Select();
            junction.DebugColor = Color.Red;
            Biome[] affectedBiomes;
            if (SettleableBiomes.TryGetValue(junction.Size, out affectedBiomes)) 
            {
                foreach (var zone in junction.Zones)
                {
                    if (affectedBiomes.Contains(zone.Biome) && zone.SettlementSize < junction.Size)
                    {
                        zone.SettlementSize = junction.Size;
                        zone.DebugColor = Color.Red;
                    }
                }
            }

            // Remove all nearby junctions from the candidate list.
            Junctions.RemoveAll((j) => new Vector(j, junction).Length < MinimumDistance);
        }
    }
}
