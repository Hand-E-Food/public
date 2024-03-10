using RandomVectorMap.Mapping;

namespace RandomVectorMap.Navigation
{
    partial class JunctionToJunctionNavigator
    {

        /// <summary>
        /// A candidate route through the map.
        /// </summary>
        public class Route : Navigation.Route
        {

            /// <summary>
            /// Initialises a new instance of the <see cref="JunctionToJunctionNavigator.Route"/> class.
            /// </summary>
            /// <param name="origin">The origin junction of this route.</param>
            /// <param name="remainder">The straight-line remaining distance btween this route's end junction
            /// and the closest destination junction.</param>
            public Route(Junction origin, double remainder) : base(origin)
            {
                this.Remainder = remainder;
            }
            /// <summary>
            /// Initialises a new instance of the <see cref="JunctionToJunctionNavigator.Route"/> class.
            /// </summary>
            /// <param name="origin">The origin junction of this route.</param>
            /// <param name="parent">The parent route to this route.</param>
            /// <param name="road">The road to extend along.</param>
            /// <param name="remainder">The straight-line remaining distance btween this route's end junction
            /// and the closest destination junction.</param>
            public Route(Junction origin, Route parent, Road road, double remainder)
                : base(origin, parent, road)
            {
                this.Remainder = remainder;
            }

            #region Properties ...

            /// <summary>
            /// Gets the remaining straight-line distance from the end of this route to the dstination.
            /// </summary>
            /// <value>The straight-line distance from the end of this route to the dstination.</value>
            public double Remainder { get; private set; }

            /// <summary>
            /// Gets this state's score as a solution candidate.  Lower scores are more likely to lead to a solution.
            /// </summary>
            /// <value>This state's score as a solution candidate.</value>
            public override double Score { get { return base.Score + Remainder; } }

            #endregion
        }
    }
}
