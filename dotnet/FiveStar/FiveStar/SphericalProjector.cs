using static System.Math;

namespace FiveStar
{
    public class SphericalProjector : IProjector
    {
        private const double Radians = PI / 180.0;

        private readonly SphericalPoint o;

        public SphericalProjector(SphericalPoint origin)
        {
            o = origin;
            o.θ *= Radians;
            o.φ *= Radians;
        }

        public NamedPoint Project(SphericalPoint s)
        {
            // x-axis: horizontal on the 2D plane
            // y-axis: vertical on the 2D plane
            // z-axis: perpendicular to the 2D plane

            s.θ *= Radians;
            s.φ *= Radians;

            // Rotate around the y-axis so the origin is θ = 0°.
            s.θ -= o.θ;

            // Radius from y-axis.
            var yr = s.ρ * Cos(s.φ);

            // Calculate cartesian coordinates before rotating around x-axis.
            var x = yr * Sin(s.θ);
            var y = yr * Tan(s.φ);
            var z = yr * Cos(s.θ);

            // Radius from x-axis.
            var xr = Sqrt(y * y + z * z);

            // Angle from x-axis, 0° on the z-axis.
            var xα = Atan(y / z);

            // Rotate around the x-axis so the origin is φ = 0°.
            y = xr * Sin(xα - o.φ);
            // z also changes in the rotation, but it's not used.

            return new NamedPoint(x, -y, s.Name);
        }
    }
}
