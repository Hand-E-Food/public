using System;
using System.Drawing;

namespace RandomVectorMap.Generation
{
    /// <summary>
    /// Contains logic for generating a zone's third junction based on two other junctions.
    /// </summary>
    public abstract class TriangleGenerator : IRandomized
    {

        #region Properties ...

        /// <summary>
        /// Gets or sets the maximum length of a road.
        /// </summary>
        public double MaximumRoadLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of a road.
        /// </summary>
        public double MinimumRoadLength { get; set; }

        /// <summary>
        /// The map to generate.
        /// </summary>
        public Random Random { get; set; }

        #endregion

        /// <summary>
        /// Generates the junction that closes the new zone.
        /// </summary>
        /// <param name="pointA">The triangle's first vertex.</param>
        /// <param name="pointB">The triangle's second vertex.</param>
        /// <param name="pointBias">The point away from which the triangle's thrid point should be created.</param>
        /// <returns>The triangle's third vertex.</returns>
        public abstract Point GenerateTriangle(Point pointA, Point pointB, Point pointBias);
    }
}
