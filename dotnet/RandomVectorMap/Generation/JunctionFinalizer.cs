using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Sets all undefined junctions to empty junctions.
    /// </summary>
    public class JunctionFinalizer : SingleStepMapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="JunctionFinalizer"/> class.
        /// </summary>
        public JunctionFinalizer()
        {
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Set all undefined junctions to no settlement.
            foreach (var junction in Map.Junctions.Where((j) => j.Size == SettlementSize.Undefined))
            {
                junction.Size = SettlementSize.None;
            }
            // This task is finished.
            base.Step();
        }
    }
}
