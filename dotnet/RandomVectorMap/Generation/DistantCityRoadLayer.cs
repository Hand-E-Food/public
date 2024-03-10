using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// 
    /// </summary>
    public class DistantCityRoadLayer : RoadLayer
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomVectorMap.Generation.DistantCityRoadLayer"> class.
        /// </summary>
        public DistantCityRoadLayer()
        {
            AlternateRouteRatio = 1.0;
            CasualDistance = double.PositiveInfinity;
            Routes = new SortedSet<Route>();
            SettlementSizes = new HashSet<SettlementSize>();
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the ratio between a route and an alternate route that would make the route redundant.
        /// </summary>
        /// <value>The ratio between a route and an alternate route that would make the route redundant.</value>
        public double AlternateRouteRatio { get; set; }

        /// <summary>
        /// Gets or sets the distance between settlements within which a road will be laid regardless of
        /// other factors.
        /// </summary>
        /// <value>The maximum distance between settlements within which a road will be laid regardless of
        /// other factors.</value>
        public double CasualDistance { get; set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && Routes.Count <= 0; } }

        /// <summary>
        /// The list of routes between settlements.
        /// </summary>
        /// <value>A sorted list of routes between each settlement.</value>
        private SortedSet<Route> Routes { get; set; }

        /// <summary>
        /// Gets a collection of settlement sizes that are included.
        /// </summary>
        /// <value>A collection of settlement sizes.</value>
        public HashSet<SettlementSize> SettlementSizes { get; private set; }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Find all settlements of the appropriate size.
            var settlements = Map.Junctions.Where((j) => SettlementSizes.Contains(j.Size)).ToArray();

            // Find routes between each pair of networks.  Multi-threaded.
            Routes = new SortedSet<Route>();
            var settlementPairs = new List<Tuple<Junction, Junction>>();
            for (int i = 0; i < settlements.Length; i++)
            {
                for (int j = i + 1; j < settlements.Length; j++)
                {
                    settlementPairs.Add(Tuple.Create(settlements[i], settlements[j]));
                }
            }
            Parallel.ForEach(settlementPairs, ((p) => InitializeRoute(p.Item1, p.Item2)));
        }

        /// <summary>
        /// Finds a route between two road networks.
        /// </summary>
        /// <param name="origin">The origin junction.</param>
        /// <param name="target">The target junction.</param>
        /// <returns>The best route found between the two junctions.  Null if no route was found.</returns>
        private void InitializeRoute(Junction origin, Junction target)
        {
            // Find the best route between the two junctions.
            var navigator = new JunctionToJunctionNavigator(new[] { origin }, new[] { target });
            navigator.RoadQualities.AddRange(AllowedRoadQualities);
            navigator.Solve();
            // Record the best route, if any.
            var route = navigator.BestRoute;
            if (route != null)
            {
                bool lockTaken = false;
                try
                {
                    LockForInitializeRoute.Enter(ref lockTaken);
                    Routes.Add(route);
                }
                finally
                {
                    LockForInitializeRoute.Exit();
                }
            }
        }
        private SpinLock LockForInitializeRoute = new SpinLock();

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Get the shortest linking route.
            var route = Routes.First();
            Routes.Remove(route);
            // Show the proposed route.
            foreach (var road in route.Roads)
            {
                road.DebugColor = Color.Blue;
            }
            // Check if the route is longer than a casual route.
            Route alternateRoute = null;
            if (route.Length > CasualDistance)
            {   // If the route is longer than a casual route...
                // Check if there is an alternate route for the route.
                var navigator = new JunctionToJunctionNavigator(new[] { route.Junctions.First() }, new[] { route.Junctions.Last() });
                navigator.Maximum = route.Length * AlternateRouteRatio;
                navigator.RoadQualities.Add(LaidRoadQuality);
                navigator.Solve();
                alternateRoute = navigator.BestRoute;
            }
            if (alternateRoute == null)
            {   // If there is no alternate route or this route is of casual distance...
                // Lay the road.
                LayRoad(route.Roads);
            }
            else
            {   // If there is an alternate route...
                // Show the alternate route.
                foreach (var road in alternateRoute.Roads)
                    road.DebugColor = Color.Red;
            }
        }
    }
}
