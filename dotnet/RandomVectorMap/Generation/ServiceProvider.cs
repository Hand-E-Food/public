using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RandomVectorMap.Mapping;
using RandomVectorMap.Navigation;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// 
    /// </summary>
    public class ServiceProvider : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomVectorMap.Generation.ServiceProvider"> class.
        /// </summary>
        public ServiceProvider()
        {
            AllowedRoadQualities = new HashSet<RoadQuality>();
            AllowedSettlementSizes = new HashSet<SettlementSize>();
            MaximumDistance = double.MaxValue;
            SettlementSize = SettlementSize.Undefined;
        }

        #region Properties ...

        /// <summary>
        /// Gets the collection of road qualities that may be traversed.
        /// </summary>
        /// <value>A collection of road qualities.</value>
        public HashSet<RoadQuality> AllowedRoadQualities { get; private set; }

        /// <summary>
        /// Gets the collection of settlement sizes that can provide service.
        /// </summary>
        /// <value>A collection of settlement sizes.</value>
        public HashSet<SettlementSize> AllowedSettlementSizes { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        public override bool IsFinished 
        { 
            get 
            {
                return IsInitialized
                    && (JunctionDistances.Count <= 0 || JunctionDistances.First().Distance < MaximumDistance);
            }
        }

        /// <summary>
        /// A collection of junctions and their distance from a service station.
        /// </summary>
        private SortedSet<JunctionDistance> JunctionDistances { get; set; }

        /// <summary>
        /// Gets or sets the maximum distance between service providers.
        /// </summary>
        public double MaximumDistance { get; set; }

        /// <summary>
        /// The junctions that provide service.
        /// </summary>
        private HashSet<Junction> ServiceJunctions { get; set; }

        /// <summary>
        /// Gets or sets the settlement size to build.
        /// </summary>
        public SettlementSize SettlementSize { get; set; }

        #endregion

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            InitializeServiceJunctions();
            InitializeJunctionDistances();
        }

        /// <summary>
        /// Initialises the collection of junction distances.
        /// </summary>
        private void InitializeJunctionDistances()
        {
            // Initialise the collection.
            JunctionDistances = new SortedSet<JunctionDistance>();
            // Find all junctions with no settlement and at least one road.
            var junctions =
                Map.Junctions
                .Where((j) =>
                    j.Size == SettlementSize.Undefined
                &&
                    j.Roads
                    .Any((r) => AllowedRoadQualities.Contains(r.Quality))
                );
            // Measure the distance from each junction to the nearest service station.
            Parallel.ForEach(junctions, (junction) =>
            {
                var distance = MeasureDistanceToService(junction);
                bool lockTaken = false;
                try
                {
                    LockForInitializeJunctionDistances.Enter(ref lockTaken);
                    JunctionDistances.Add(new JunctionDistance(junction, distance));
                }
                finally
                {
                    LockForInitializeJunctionDistances.Exit();
                }
            });
        }
        private SpinLock LockForInitializeJunctionDistances = new SpinLock();

        /// <summary>
        /// Initialises the collection of service junctions.
        /// </summary>
        private void InitializeServiceJunctions()
        {
            var serviceJunctions =
                Map.Junctions
                .Where((j) => AllowedSettlementSizes.Contains(j.Size));
            ServiceJunctions = new HashSet<Junction>(serviceJunctions);
        }

        /// <summary>
        /// Measures the distance from the specified junction to the nearest service station.
        /// </summary>
        /// <param name="junction">The junction to search from.</param>
        /// <returns>The distance from the junction to the nearest service station.</returns>
        private double MeasureDistanceToService(Junction junction)
        {
            var navigator = new JunctionToJunctionNavigator(new[] { junction }, ServiceJunctions);
            navigator.RoadQualities.AddRange(AllowedRoadQualities);
            navigator.Solve();

            return navigator.BestRoute.Length;
        }

        /// <summary>
        /// Performs a single step of map generation.
        /// </summary>
        public override void Step()
        {
            // Get the junction furthest from any service station.
            var junction = JunctionDistances.First();
            JunctionDistances.Remove(junction);
            // Remeasure the junction's distance from the nearest service station.
            var oldDistance = junction.Distance;
            var navigator = new JunctionToJunctionNavigator(new Junction[] { junction }, ServiceJunctions);
            navigator.RoadQualities.AddRange(AllowedRoadQualities);
            navigator.Solve();
            var newDistance = navigator.BestRoute.Length;
            // Check if the junction is now closer to a service station than when last measured.
            if (newDistance == oldDistance)
            {   // If this junction is still the furthest from a service station...
                // Create a service station here.
                junction.Junction.Size = SettlementSize;
            }
            else
            {   // If this junction is closer to a service station than last measured...
                // Reinsert it into the collection.
                junction.Distance = newDistance;
                JunctionDistances.Add(junction);
            }
            // Mark debugging information.
            junction.Junction.DebugColor = Color.Red;
            foreach (var road in navigator.BestRoute.Roads)
            {
                road.DebugColor = Color.Red;
            }

        }

        /// <summary>
        /// Information related to a junction.
        /// </summary>
        private class JunctionDistance : IComparable<JunctionDistance>
        {

            /// <summary>
            /// Initialises a new instnace of the <see cref="JunctionDistance"/> class.
            /// </summary>
            /// <param name="junction">The junction.</param>
            /// <param name="distance">The distance this junction is from any service station.</param>
            public JunctionDistance(Junction junction, double distance)
            {
                Junction = junction;
                Distance = distance;
            }

            #region Properties ...

            /// <summary>
            /// Gets the junction.
            /// </summary>
            public Junction Junction { get; private set; }

            /// <summary>
            /// Gets or sets the distance this junction is from any service station.
            /// </summary>
            public double Distance { get; set; }

            #endregion

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>A value that indicates the relative order of the objects being compared.</returns>
            public int CompareTo(JunctionDistance other)
            {
                return other.Distance.CompareTo(this.Distance);
            }

            /// <summary>
            /// Converts this object to a Junction.
            /// </summary>
            /// <param name="obj">The object to convert.</param>
            /// <returns>The object's junction.</returns>
            public static implicit operator Junction(JunctionDistance obj)
            {
                return obj.Junction;
            }
        }
    }
}
