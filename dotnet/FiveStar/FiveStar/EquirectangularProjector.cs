using static System.Math;

namespace FiveStar
{
    public class EquirectangularProjector : IProjector
    {
        private const double Radians = PI / 180.0;

        /// <summary>
        /// The <see cref="SphericalPoint"/> to use as the origin for projections.
        /// </summary>
        private readonly SphericalPoint o;

        private readonly double xRatio;
        private readonly double yRatio;

        /// <summary>
        /// Initialises a new instance of the <see cref="EquirectangularProjector"/> class.
        /// </summary>
        /// <param name="origin">The <see cref="SphericalPoint"/> to use as the origin for projections.</param>
        public EquirectangularProjector(SphericalPoint origin)
        {
            o = origin;
            xRatio = Cos(o.φ * Radians) * Radians;
            yRatio = -Radians;
        }

        /// <summary>
        /// Projects the specified point onto a flat surface.
        /// </summary>
        /// <param name="s">The <see cref="SphericalPoint"/> to project.</param>
        /// <returns>The <see cref="NamedPoint"/> where <paramref name="s"/> is shown on a flat surface.</returns>
        public NamedPoint Project(SphericalPoint s) => new NamedPoint(
            (s.ρ * (s.θ - o.θ) * xRatio),
            (s.ρ * (s.φ - o.φ) * yRatio),
            s.Name
        );
    }
}
