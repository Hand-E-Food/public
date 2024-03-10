using System.Drawing;
using System.Linq;

namespace RandomVectorMap.Mapping
{

    /// <summary>
    /// Represent a line between two points.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Origin}, {Terminus}, length {Length}")]
    public struct Line
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="point1">The line's starting point.</param>
        /// <param name="point2">The line's ending point.</param>
        public Line(Point origin, Point terminus) : this()
        {
            Origin = origin;
            Terminus = terminus;
            Vector = new Vector(origin, terminus);
            ZeroIntersect = double.IsInfinity(Gradient) ? double.NaN : (Origin.Y - Origin.X * Gradient);
        }

        #region Properties ...

        /// <summary>
        /// Gets this line's end points.
        /// </summary>
        /// <value>An array of this line's end points.</value>
        public Point[] EndPoints { get { return new[] { Origin, Terminus }; } }

        /// <summary>
        /// Gets this line's gradient relative to the X axis.
        /// </summary>
        /// <value>This line's gradient relative to the X axis.</value>
        public double Gradient { get { return Vector.Gradient; } }

        /// <summary>
        /// Gets this line's length.
        /// </summary>
        public double Length { get { return Vector.Length; } }

        /// <summary>
        /// The line's origin point.
        /// </summary>
        /// <value>The point at the start of this line.</value>
        public Point Origin { get; private set; }

        /// <summary>
        /// The line's terminus point.
        /// </summary>
        /// <value>The point at the end of this line.</value>
        public Point Terminus { get; private set; }

        /// <summary>
        /// Gets this line's vector, the difference between the two points.
        /// </summary>
        /// <value>This line's vector, the difference between the two points.</value>
        public Vector Vector { get; private set; }

        /// <summary>
        /// Gets the Y value at which X = 0.
        /// </summary>
        /// <value>The Y value at which X = 0.</value>
        public double ZeroIntersect { get; private set; }

        /// <summary>
        /// Gets this line's left most X coordinate.
        /// </summary>
        /// <value>This line's left most X coordinate.</value>
        public int Left { get { return EndPoints.Min((p) => p.X); } }
        /// <summary>
        /// Gets this line's right most X coordinate.
        /// </summary>
        /// <value>This line's right most X coordinate.</value>
        public int Right { get { return EndPoints.Max((p) => p.X); } }
        /// <summary>
        /// Gets this line's top most Y coordinate.
        /// </summary>
        /// <value>This line's top most Y coordinate.</value>
        public int Top { get { return EndPoints.Min((p) => p.Y); } }
        /// <summary>
        /// Gets this line's bottom most Y coordinate.
        /// </summary>
        /// <value>This line's bottom most Y coordinate.</value>
        public int Bottom { get { return EndPoints.Max((p) => p.Y); } }

        #endregion

        /// <summary>
        /// Checks if this line intersects with another line.
        /// </summary>
        /// <param name="other">The other line to compare.</param>
        /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
        public bool Intersects(Line other)
        {
            // Determine if the two line segments share an end point.
            if (this.EndPoints.Intersect(other.EndPoints).Count() > 0)
            {   // If the two line segments share an end point...
                return this.IntersectsSequential(other);
            }
            // Determine which lines are vertical.
            else if (double.IsInfinity(this.Gradient))
            {
                if (double.IsInfinity(other.Gradient))
                {   // If both lines are vertical...
                    return this.IntersectsBothVertical(other);
                }
                else
                {   // If this line is vertical and the other line is not...
                    return this.IntersectsVertical(other);
                }
            }
            else
            {
                if (double.IsInfinity(other.Gradient))
                {   // If the other line is vertical and this line is not...
                    return other.IntersectsVertical(this);
                }
                else
                {   // If neither line is vertical...
                    return IntersectsGradient(other);
                }
            }
        }
        /// <summary>
        /// Checks if this line intersects with another line when the line segments share an end point.
        /// </summary>
        /// <param name="other">The other line to compare.</param>
        /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
        private bool IntersectsSequential(Line other)
        {
            return (double.IsInfinity(this.Gradient) && double.IsInfinity(other.Gradient))
                || this.Gradient == other.Gradient;
        }
        /// <summary>
        /// Checks if this line intersects with another line when neither line is vertical.
        /// </summary>
        /// <param name="other">The other line to compare.</param>
        /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
        private bool IntersectsGradient(Line other)
        {
            // Find the X coordinate where the lines cross.
            double x = (other.ZeroIntersect - this.ZeroIntersect) / (this.Gradient - other.Gradient);
            // If the X coordinate is outside of either line segment, the line segments do not intersect.
            if (x <= this.Left || x >= this.Right) return false;
            if (x <= other.Left || x >= other.Right) return false;
            return true;
        }
        /// <summary>
        /// Checks if this line intersects with another line when this line is vertical and the other line is not.
        /// </summary>
        /// <param name="other">The other line to compare.</param>
        /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
        private bool IntersectsVertical(Line other)
        {
            // Check if the other line segment lies on both sides of this line.
            if (other.Left < this.Origin.X && other.Right > this.Origin.X)
            {   // If the other line segment lies on both sides of this line...
                // Find the Y coordinate where the lines cross.
                int y = (int)(other.ZeroIntersect + other.Gradient * this.Origin.X);
                // If the Y coordinate is within this line segment, the line segments intersect.
                return y > this.Top
                    && y < this.Bottom;
            }
            else
            {   // If the other line segment lies entirely on one side of this line...
                // The line segments do not intersect.
                return false;
            }
        }
        /// <summary>
        /// Checks if this line intersects with another line when both lines are vertical.
        /// </summary>
        /// <param name="other">The other line to compare.</param>
        /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
        private bool IntersectsBothVertical(Line other)
        {
            // If the lines have different X coordinates, the line segments do not intersect.
            if (other.Origin.X != this.Origin.X) return false;
            // The line segments intersect only if they have overlapping Y coordinates.
            return this.Top < other.Bottom
                || this.Bottom > other.Top;
        }

        /// <summary>
        /// Returns the origin or the terminus that isn't the specified point.
        /// </summary>
        /// <param name="point">The point to not find.</param>
        /// <returns>The origin or the terminus that isn't the specified point.</returns>
        public Point Other(Point point)
        {
            return new[] { Origin, Terminus }.First((p) => p != point);
        }
    }
}
