using RandomVectorMap.Mapping;
using System.Drawing;
using System.Linq;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Identifies the outside zone after it has been reclassified as a lake.
    /// </summary>
    public class OutsideIdentifier : SingleStepMapGeneratorComponent 
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomVectorMap.OutsideIdentifier"> class.
        /// </summary>
        public OutsideIdentifier()
        {
            Biome = Biome.Undefined;
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the biome for the Outside zone.
        /// </summary>
        /// <value>A biome.</value>
        public Biome Biome { get; set; }

        #endregion

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Assign the Outside zone.
            Map.InitializeOutside();
            Map.Outside.Biome = Biome;
            // Set debugging information.
            foreach (var road in Map.Outside.Roads)
            {
                road.DebugColor = Color.Blue;
            }
            foreach (var junction in Map.Outside.Junctions)
            {
                junction.DebugColor = Color.Blue;
            }
            base.Step();
        }
    }
}
