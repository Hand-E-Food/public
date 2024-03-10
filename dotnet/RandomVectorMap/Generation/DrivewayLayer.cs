using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// Lays driveways between settlements and main roads.
    /// </summary>
    public class DrivewayLayer : RoadLayer
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="DrivewayLayer"/> class.
        /// </summary>
        public DrivewayLayer()
        {
            Index = 0;
            SettlementSizes = new HashSet<SettlementSize>();
            TargetRoadQualities = new HashSet<RoadQuality>();
            TargetSettlementSizes = new HashSet<SettlementSize>();
        }

        #region Properties ...

        /// <summary>
        /// The settlement index counter.
        /// </summary>
        /// <value>An index in the Settlements array.</value>
        private int Index { get; set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished { get { return IsInitialized && Index >= Settlements.Length; } }

        /// <summary>
        /// The settlements to join.
        /// </summary>
        /// <value>An array of settlements.</value>
        private Junction[] Settlements { get; set; }

        /// <summary>
        /// The list of settlement sizes to include.
        /// </summary>
        /// <value>A collection of settlement sizes.</value>
        public HashSet<SettlementSize> SettlementSizes { get; private set; }

        /// <summary>
        /// Gets the collection of road qualities that this road should connect to.
        /// </summary>
        public HashSet<RoadQuality> TargetRoadQualities { get; private set; }

        /// <summary>
        /// The list of target settlement sizes to include.
        /// </summary>
        /// <value>A collection of settlement sizes.</value>
        public HashSet<SettlementSize> TargetSettlementSizes { get; private set; }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Settlements = Map.Junctions.Where((j) => SettlementSizes.Contains(j.Size)).ToArray();
        }

        /// <summary>
        /// Lays a road between the two junctions.
        /// </summary>
        /// <param name="origin">The road's origin.</param>
        /// <param name="destination">The road's destination.</param>
        private void LayRoad(Junction origin)
        {
            var navigator = new JunctionToJunctionNavigator(
                new[] {origin},
                Map.Junctions.Where((j) => TargetSettlementSizes.Contains(j.Size) || j.Roads.Select((r) => r.Quality).Intersect(TargetRoadQualities).Count() > 0).ToArray()
            );
            navigator.RoadQualities.AddRange(AllowedRoadQualities);

            navigator.Solve();
            if (navigator.BestRoute == null) return;

            LayRoad(navigator.BestRoute.Roads);

            foreach (var road in navigator.BestRoute.Roads)
            {
                road.DebugColor = Color.Blue;
            }
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            LayRoad(Settlements[Index++]);
        }
    }
}
