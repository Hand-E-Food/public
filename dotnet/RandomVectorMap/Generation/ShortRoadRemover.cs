using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Removes short roads from the map.
    /// </summary>
    public class ShortRoadRemover : SingleStepMapGeneratorComponent
    {

        #region Properties ...

        /// <summary>
        /// Gets or sets the minimum allowable road length.
        /// </summary>
        /// <value>The minimum allowable road length.</value>
        public double MinimumRoadLength { get; set; }

        #endregion

        /// <summary>
        /// Performs a single step of its task.
        /// </summary>
        public override void Step()
        {
            // Remove all roads shorter than the minimum length.
            foreach (var road in Map.Roads.Where((r) => r.Length < MinimumRoadLength && r.Quality == RoadQuality.Undefined))
            {
                road.Quality = RoadQuality.None;
                road.DebugColor = Color.Red;
            }
            base.Step();
        }
    }
}
