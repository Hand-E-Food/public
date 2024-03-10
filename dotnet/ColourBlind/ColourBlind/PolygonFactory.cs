using static System.Math;

namespace ColourBlind
{
    /// <summary>
    /// A factory for creating different shaped polygons.
    /// </summary>
    public class PolygonFactory
    {
        public PolygonD AlignedIcon { get; }
        public PolygonD CornersIcon { get; }
        public PolygonD GroupIcon { get; }
        public PolygonD IsolatedIcon { get; }

        /// <summary>
        /// The radius this factory uses to scale polygons.
        /// </summary>
        public double Radius { get; }

        /// <summary>
        /// Initialises a new <see cref="PolygonFactory"/>.
        /// </summary>
        /// <param name="radius">The radius this factory uses to scale polygons.</param>
        public PolygonFactory(double radius)
        {
            Radius = radius;
            CornersIcon = CreateCornersIcon();
            GroupIcon = CreateGroupIcon();
            AlignedIcon = CreateAlignedIcon();
            IsolatedIcon = CreateIsolatedIcon();
        }

        /// <summary>
        /// Creates a minus polygon.
        /// </summary>
        /// <returns>A minus polygon.</returns>
        private PolygonD CreateAlignedIcon()
        {
            double x = Radius * 0.8;
            double y = Radius * 0.2;
            PolygonD polygon = new();

            polygon.AddLoop(
                new PointD(-x, -y),
                new PointD(x, -y),
                new PointD(x, y),
                new PointD(-x, y)
            );

            return polygon;
        }

        /// <summary>
        /// Creates a diamond polygon.
        /// </summary>
        /// <returns>A diamond polygon.</returns>
        private PolygonD CreateCornersIcon()
        {
            double centre = Radius * 0.45;
            double radius = Radius * 0.40;
            PolygonD polygon = new();

            polygon.AddLoop(LoopCircle( centre, 0, radius, 4));
            polygon.AddLoop(LoopCircle(-centre, 0, radius, 4));
            polygon.AddLoop(LoopCircle(0,  centre, radius, 4));
            polygon.AddLoop(LoopCircle(0, -centre, radius, 4));

            return polygon;
        }

        /// <summary>
        /// Creates a donut polygon.
        /// </summary>
        /// <returns>A donut polygon.</returns>
        private PolygonD CreateGroupIcon()
        {
            PolygonD polygon = new();

            polygon.AddLoop(LoopCircle(0, 0, Radius * 0.8, 36));
            polygon.AddLoop(LoopCircle(0, 0, Radius * 0.5, 36));

            return polygon;
        }

        /// <summary>
        /// Creates a triangle polygon.
        /// </summary>
        /// <returns>A triangle polygon.</returns>
        private PolygonD CreateIsolatedIcon()
        {
            double r = Radius * 0.7;
            PolygonD polygon = new();

            polygon.AddLoop(LoopCircle(0, 0, r, 3));

            return polygon;
        }

        private PointD[] LoopCircle(double x, double y, double r, int resolution)
        {
            double resolutionArc = 2 * PI / resolution;

            return Enumerable.Range(0, resolution)
                .Select(i => {
                    double angle = i * resolutionArc;
                    return new PointD(
                        x + Cos(angle) * r,
                        y + Sin(angle) * r
                    );
                })
                .ToArray();
        }
    }
}
