using System.Collections.Generic;
using System.Drawing;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// The base class for all road laying classes.
    /// </summary>
    public abstract class RoadLayer : MapGeneratorComponent
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="RoadLayer"/> class.
        /// </summary>
        public RoadLayer()
        {
            AllowedRoadQualities = new HashSet<RoadQuality>();
            LaidRoadQuality = RoadQuality.Undefined;
        }

        #region Properties ...

        /// <summary>
        /// Gets a collection of allowed road qualities to form routes upon.
        /// </summary>
        /// <value>A collection of road qualities.</value>
        public HashSet<RoadQuality> AllowedRoadQualities { get; private set; }

        /// <summary>
        /// Gets or sets the road quality to lay.
        /// </summary>
        public RoadQuality LaidRoadQuality { get; set; }

        #endregion

        /// <summary>
        /// Lays a road along a set of road segments.
        /// </summary>
        /// <param name="roads">The road segments along which to lay the road.</param>
        protected void LayRoad(Road[] roads)
        {
            foreach (var road in roads)
            {
                if (road.Quality < LaidRoadQuality)
                    road.Quality = LaidRoadQuality;
            }
        }
    }
}
