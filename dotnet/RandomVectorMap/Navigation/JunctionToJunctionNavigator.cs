using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Navigation
{

    /// <summary>
    /// Finds a route between two specified junctions.
    /// </summary>
    public partial class JunctionToJunctionNavigator : Navigator<JunctionToJunctionNavigator.Route>
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="JunctionToJunctionNavigator"/> class.
        /// </summary>
        public JunctionToJunctionNavigator(IEnumerable<Junction> origins, IEnumerable<Junction> destinations) 
        {
            Origins = origins;
            Destinations = destinations;
            Minimum = double.NegativeInfinity;
            Maximum = double.PositiveInfinity;

            // Set the initial candidate.
            foreach (var origin in origins)
            {
                double remainder = GetRemainingDistance(origin);
                Route route = new Route(origin, remainder);
                AddCandidate(route);
            }
        }

        #region Properties ...

        /// <summary>
        /// Gets the destination junction.
        /// </summary>
        public IEnumerable<Junction> Destinations { get; private set; }

        /// <summary>
        /// Gets the inclusive maximum distance to traverse.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets the exclusive minimum distance to traverse.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets the origin junction.
        /// </summary>
        public IEnumerable<Junction> Origins { get; private set; }

        #endregion

        /// <summary>
        /// Extends a route along a specified road.
        /// </summary>
        /// <param name="route">The route to extend.</param>
        /// <param name="road">The road used to extend the route.</param>
        /// <returns>The extended route.</returns>
        protected override Route ExtendRoute(Route route, Road road)
        {
            double remainder = GetRemainingDistance(road.Other(route.Junctions.Last()));
            Route newRoute = new Route(route.Junctions.First(), route, road, remainder);
            return newRoute;
        }

        /// <summary>
        /// Gets the distance between the specified point and the destination.
        /// </summary>
        /// <param name="routeEnd">The end of the route.</param>
        /// <returns>The distance between the specified point and the destination.</returns>
        private double GetRemainingDistance(Junction routeEnd)
        {
            return Destinations.Min((destination) => new Vector(routeEnd, destination).Length);
        }
 
        /// <summary>
        /// Checks whether a candidate is a viable solution.
        /// </summary>
        /// <param name="route">The candidate to test.</param>
        /// <returns>True if the candidate is a viable solution; otherwise, false.</returns>
        protected override bool IsSolution(Route route)
        {
            return route.Remainder == 0;
        }

        /// <summary>
        /// Determines whether the state's route is too long to be considered as a candidate.
        /// </summary>
        /// <param name="route">The state to test.</param>
        /// <returns>True if the state's route is too long to be considered as a candidate; otherwise, false.</returns>
        protected override bool IsViable(Route route)
        {
            return base.IsViable(route)
                && (BestRoute == null || route.Score <= BestRoute.Length)
                && route.Score <= Maximum
                && (route.Remainder > 0 || route.Score > Minimum);
        }
   }
}
