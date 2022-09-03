using static System.Math;

namespace ColourBlind
{
    /// <summary>
    /// Creates a cloneable template.
    /// </summary>
    internal class TemplateFactory
    {
        /// <summary>
        /// The minimum margin between circles.
        /// </summary>
        private const double CircleMargin = TemplateRadius * 0.005;

        /// <summary>
        /// The maximum radius of a circle.
        /// </summary>
        private const double MaxCircleRadius = TemplateRadius * 0.015;

        /// <summary>
        /// The minimum radius of a circle.
        /// </summary>
        private const double MinCircleRadius = TemplateRadius * 0.005;

        /// <summary>
        /// The maximum number of attempts to find a new place for a circle.
        /// </summary>
        private const int MaxAttempts = 10000;

        /// <summary>
        /// The template's diameter.
        /// </summary>
        public const float TemplateDiameter = TemplateRadius * 2;

        /// <summary>
        /// The template's margin.
        /// </summary>
        public const float TemplateMargin = TemplateRadius * 0.25f;

        /// <summary>
        /// The template's radius.
        /// </summary>
        public const float TemplateRadius = 800;

        /// <summary>
        /// The template's radius squared.
        /// </summary>
        private const float TemplateRadiusSquared = TemplateRadius * TemplateRadius;

        /// <summary>
        /// Creates a cloneable colour blind template.
        /// </summary>
        /// <returns>The colour blind template.</returns>
        public Template CreateTemplate()
        {
            Template template = new(TemplateRadius, TemplateMargin);
            Random random = new(0);
            int attempts = MaxAttempts;
            while (attempts-- > 0)
            {
                double x = random.NextDouble() * TemplateDiameter - TemplateRadius;
                double yMax = Sqrt(TemplateRadiusSquared - x * x);
                double y = random.NextDouble() * yMax * 2 - yMax;
                double r = Min(MaxCircleRadius, TemplateRadius - Range(x, y));
                foreach (var other in template.Circles)
                {
                    PointD centre = other.Centre;
                    double maxRange = r + other.Radius + CircleMargin;
                    if (MaxMagnitude(centre.X - x, centre.Y - y) >= maxRange) continue;
                    r = Min(r, Range(x, y, centre.X, centre.Y) - other.Radius - CircleMargin);
                    if (r < MinCircleRadius) break;
                }
                if (r < MinCircleRadius) continue;

                double hue = random.NextDouble();
                double saturation = random.NextDouble() * 0.3 + 0.2;
                double lightness = random.NextDouble() * 0.3 + 0.5;

                template.Add(new Circle(new PointD(x, y), r, saturation, lightness, hue));
                attempts = MaxAttempts;
            }
            return template;
        }

        /// <summary>
        /// Gets the distance between two pairs of coorrdinates.
        /// </summary>
        /// <param name="x1">The first x coordinate.</param>
        /// <param name="y1">The first y coordinate.</param>
        /// <param name="x2">The second x coordinate.</param>
        /// <param name="y2">The second y coordinate.</param>
        /// <returns>The distance between (<paramref name="x1"/>, <paramref name="y1"/>) and (<paramref name="x2"/>, <paramref name="y2"/>).</returns>
        private double Range(double x1, double y1, double x2, double y2) => Range(x2 - x1, y2 - y1);

        /// <summary>
        /// Gets the distance to the specified coordinates from (0, 0).
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The distance between (0, 0) and (<paramref name="x"/>, <paramref name="y"/>).</returns>
        private double Range(double x, double y) => Sqrt(x * x + y * y);
    }
}
