using Svg;
using System;

namespace ColourBlind
{
    /// <summary>
    /// A colour blind template.
    /// </summary>
    public class Template : ICloneable
    {
        private readonly double margin;

        /// <summary>
        /// This template's circles.
        /// </summary>
        public List<Circle> Circles { get; } = new();

        /// <summary>
        /// This template's diameter.
        /// </summary>
        public double Radius { get; }

        /// <summary>
        /// This template's SVG document.
        /// </summary>
        public SvgDocument Svg { get; }

        /// <summary>
        /// Initialises a new <see cref="Template"/>.
        /// </summary>
        /// <param name="radius">The template's diameter.</param>
        public Template(double radius, double margin)
        {
            Radius = radius;
            this.margin = margin;
            float r = (float)(radius + margin);
            float d = r * 2;
            Svg = new SvgDocument
            {
                StrokeWidth = 0f,
                ViewBox = new(-r, -r, d, d),
            };
        }

        /// <summary>
        /// Adds a circle to this template.
        /// </summary>
        /// <param name="circle">The circle to add.</param>
        public void Add(Circle circle)
        {
            Circles.Add(circle);
            Svg.Children.Add(circle.Svg);
        }

        /// <summary>
        /// Returns a deep copy of this object.
        /// </summary>
        /// <returns>A deep copy of this object.</returns>
        public Template Clone()
        {
            Template clone = new(Radius, margin);

            foreach (var circle in Circles)
                clone.Add(circle.Clone());

            return clone;
        }
        object ICloneable.Clone() => Clone();
    }
}
