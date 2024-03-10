using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;
using System.Drawing;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Removes roads where alternate routes will suffice.
    /// </summary>
    public class RedundantRoadRemover : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RedundantRoadRemover"/> class.
        /// </summary>
        public RedundantRoadRemover()
        {
            Index = 0;
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the ratio between a road and an alternate route that would make the road redundant.
        /// </summary>
        /// <value>The ratio between a road and an alternate route that would make the road redundant.</value>
        public double AlternateRouteRatio { get; set; }

        /// <summary>
        /// The index of the next road to process.
        /// </summary>
        /// <value>The current index of the Roads list.</value>
        private int Index { get; set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && Index >= Roads.Count; } }

        /// <summary>
        /// The list of all roads sorted by longest first.
        /// </summary>
        /// <value>A list of roads.</value>
        private List<Road> Roads { get; set; }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Collect a list of all roads, sorted by longest first.
            Roads = new List<Road>(Map.Roads.Where((r) => r.Quality == RoadQuality.Undefined));
            Roads.Sort((a, b) => b.Length.CompareTo(a.Length));
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Select the road to process.
            Road road = Roads[Index];

            // Find alternate routes to this road.
            var navigator = new JunctionToJunctionNavigator(
                new[] { road.Junctions.First() }, 
                new[] { road.Junctions.Last() }
            );
            navigator.Minimum = road.Length;
            navigator.Maximum = road.Length * AlternateRouteRatio;
            navigator.RoadQualities.AddRange(new[] { 
                RoadQuality.Undefined, 
                RoadQuality.Dirt, 
                RoadQuality.Paved, 
                RoadQuality.Highway
            });
            navigator.Solve();
            // If an alternate route is found, ensure there is no road here.
            if (navigator.BestRoute != null)
            {   // If an alternate route is found...
                // Ensure there is no road here.
                road.Quality = RoadQuality.None;
                road.DebugColor = Color.Red;
                foreach (var otherRoad in navigator.BestRoute.Roads)
                {
                    otherRoad.DebugColor = Color.Blue;
                }
            }
            else
            {   // If no alternate route is found...
                // Keep this route.
                road.DebugColor = Color.Green;
            }

            // Move to the next road.
            Index++;
        }
    }
}
