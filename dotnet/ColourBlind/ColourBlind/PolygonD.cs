using Svg;
using System.Drawing;

namespace ColourBlind
{
    /// <summary>
    /// A 2-dimensional polygon.
    /// </summary>
    public class PolygonD
    {
        /// <summary>
        /// This polygon's lines.
        /// </summary>
        public IReadOnlyList<LineD> Lines => lines;
        private readonly List<LineD> lines = new();

        public SvgGroup Svg { get; }

        /// <summary>
        /// Initialises a new <see cref="PolygonD"/>.
        /// </summary>
        public PolygonD()
        {
            Svg = new()
            {
                FillOpacity = 0,
                Stroke = new SvgColourServer(Color.Black),
                StrokeWidth = 10,
            };
        }

        /// <summary>
        /// Adds a loop to this polygon.
        /// </summary>
        /// <param name="points">The sequential verticies of the loop. The last point will be connected to the first point.</param>
        /// <exception cref="ArgumentException">Less than 3 points were specified.</exception>
        public void AddLoop(params PointD[] points)
        {
            if (points.Length < 3) throw new ArgumentException("A loop must consist of at least 3 points.");

            SvgPolygon svg = new()
            {
                Points = new(),
            };
            for (int i = 0; i < points.Length; i++)
            {
                int j = i + 1;
                if (j == points.Length) j = 0;
                PointD point1 = points[i];
                PointD point2 = points[j];
                lines.Add(new LineD(point1, point2));
                svg.Points.Add((float)point1.X);
                svg.Points.Add((float)point1.Y);
            }
            Svg.Children.Add(svg);
        }

        /// <summary>
        /// Tests if the specified point is inside this polygon.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <returns>True if <paramref name="point"/> is inside this polygon. Otherwise, false.</returns>
        public bool Contains(PointD point) => Lines.Count(line => line.IsBelow(point)) % 2 == 1;
    }
}
