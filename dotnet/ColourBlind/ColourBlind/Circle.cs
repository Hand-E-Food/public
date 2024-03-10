using Svg;

namespace ColourBlind
{
    public class Circle : ICloneable
    {
        /// <summary>
        /// This circle's colour hue from 0 to 360.
        /// </summary>
        public double Hue
        {
            get => hue;
            set
            {
                hue = value;
                RefreshFillColour();
            }
        }
        private double hue = 0;

        /// <summary>
        /// This circle's colour lightness from 0.0 to 1.0.
        /// </summary>
        public double Lightness
        {
            get => lightness;
            set
            {
                lightness = value;
                RefreshFillColour();
            }
        }
        private double lightness;

        /// <summary>
        /// This circle's centre point.
        /// </summary>
        public PointD Centre { get; }

        /// <summary>
        /// The players that need to see this circle.
        /// </summary>
        public PlayerId PlayerId { get; set; } = PlayerId.None;

        /// <summary>
        /// This circle's radius.
        /// </summary>
        public double Radius { get; }

        /// <summary>
        /// This circle's colour saturation from 0.0 to 1.0.
        /// </summary>
        public double Saturation
        {
            get => saturation;
            set
            {
                saturation = value;
                RefreshFillColour();
            }
        }
        private double saturation;

        /// <summary>
        /// This circle's SVG element.
        /// </summary>
        public SvgCircle Svg { get; }

        /// <summary>
        /// Initialises a new <see cref="Circle"/>.
        /// </summary>
        /// <param name="x">The x coordinate of this circle's centre.</param>
        /// <param name="y">The y coordinate of this circle's centre.</param>
        /// <param name="radius">This circle's radius.</param>
        public Circle(PointD centre, double radius, double saturation, double lightness, double hue = 0)
        {
            Centre = centre;
            Radius = radius;
            Svg = new SvgCircle
            {
                CenterX = (float)centre.X,
                CenterY = (float)centre.Y,
                Radius = (float)radius,
            };
            this.hue = hue;
            this.saturation = saturation;
            this.lightness = lightness;
            RefreshFillColour();
        }

        /// <summary>
        /// Returns a deep copy of this object.
        /// </summary>
        /// <returns>A deep copy of this object.</returns>
        public Circle Clone() => new(Centre, Radius, Saturation, Lightness, Hue);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Updates this circle's fill colour.
        /// </summary>
        private void RefreshFillColour()
        {
            Svg.Fill = new SvgColourServer(ConvertColour.FromHSL(Hue, Saturation, Lightness));
        }
    }
}
