using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Removes all undefined roads from the map.
    /// </summary>
    public class RoadFinalizer : SingleStepMapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RoadFinalizer"/> class.
        /// </summary>
        public RoadFinalizer()
        {
            LaidRoadQuality = RoadQuality.Undefined;
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the road quality to lay.
        /// </summary>
        public RoadQuality LaidRoadQuality { get; set; }

        #endregion

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Set all undefined roads to no roads.
            foreach (var road in Map.Roads.Where((r) => r.Quality == RoadQuality.Undefined))
            {
                road.Quality = LaidRoadQuality;
            }
            base.Step();
        }
    }
}
