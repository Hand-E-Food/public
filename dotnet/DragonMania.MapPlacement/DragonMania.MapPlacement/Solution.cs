using System.Collections.Generic;

namespace DragonMania.MapPlacement
{
    /// <summary>
    /// Contains details of a solution.
    /// </summary>
    public class Solution
    {
        /// <summary>
        /// This solution's map.
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// This solutions remaining buildings that did not fit on the map.
        /// </summary>
        public IEnumerable<BuildingPool> RemainingBuildings { get; set; }

        /// <summary>
        /// This solution's score.  Lower is better.
        /// </summary>
        public int Score { get; set; }
    }
}
