namespace RandomVectorMap.Mapping;

/// <summary>
/// Represent a line between two points.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{Origin}, {Terminus}, length {Length}")]
public readonly struct Line
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Line"/> class.
    /// </summary>
    /// <param name="point1">The line's starting point.</param>
    /// <param name="point2">The line's ending point.</param>
    public Line(Point origin, Point terminus)
    {
        Origin = origin;
        Terminus = terminus;
        Vector = new(origin, terminus);
        ZeroIntersect = double.IsInfinity(Gradient) ? double.NaN : (Origin.Y - Origin.X * Gradient);
    }

    /// <summary>
    /// Gets this line's end points.
    /// </summary>
    /// <value>An array of this line's end points.</value>
    public readonly Point[] EndPoints => [Origin, Terminus];

    /// <summary>
    /// Gets this line's gradient relative to the X axis.
    /// </summary>
    /// <value>This line's gradient relative to the X axis.</value>
    public readonly double Gradient => Vector.Gradient;

    /// <summary>
    /// Gets this line's length.
    /// </summary>
    public readonly double Length => Vector.Length;

    /// <summary>
    /// The line's origin point.
    /// </summary>
    /// <value>The point at the start of this line.</value>
    public readonly Point Origin;

    /// <summary>
    /// The line's terminus point.
    /// </summary>
    /// <value>The point at the end of this line.</value>
    public readonly Point Terminus;

    /// <summary>
    /// Gets this line's vector, the difference between the two points.
    /// </summary>
    /// <value>This line's vector, the difference between the two points.</value>
    public readonly Vector Vector;

    /// <summary>
    /// Gets the Y value at which X = 0.
    /// </summary>
    /// <value>The Y value at which X = 0.</value>
    public readonly double ZeroIntersect;

    /// <summary>
    /// Gets this line's left most X coordinate.
    /// </summary>
    /// <value>This line's left most X coordinate.</value>
    public readonly int Left => EndPoints.Min(point => point.X);
    /// <summary>
    /// Gets this line's right most X coordinate.
    /// </summary>
    /// <value>This line's right most X coordinate.</value>
    public readonly int Right => EndPoints.Max(point => point.X);
    /// <summary>
    /// Gets this line's top most Y coordinate.
    /// </summary>
    /// <value>This line's top most Y coordinate.</value>
    public readonly int Top => EndPoints.Min(point => point.Y);
    /// <summary>
    /// Gets this line's bottom most Y coordinate.
    /// </summary>
    /// <value>This line's bottom most Y coordinate.</value>
    public readonly int Bottom => EndPoints.Max(point => point.Y);

    /// <summary>
    /// Checks if this line intersects with another line.
    /// </summary>
    /// <param name="other">The other line to compare.</param>
    /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
    public bool Intersects(Line other)
    {
        // Determine if the two line segments share an end point.
        if (EndPoints.Intersect(other.EndPoints).Any())
        {   // If the two line segments share an end point...
            return IntersectsSequential(other);
        }
        // Determine which lines are vertical.
        else if (double.IsInfinity(Gradient))
        {
            if (double.IsInfinity(other.Gradient))
            {   // If both lines are vertical...
                return IntersectsBothVertical(other);
            }
            else
            {   // If this line is vertical and the other line is not...
                return IntersectsVertical(other);
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
    private readonly bool IntersectsSequential(Line other)
    {
        return (double.IsInfinity(this.Gradient) && double.IsInfinity(other.Gradient))
            || this.Gradient == other.Gradient;
    }
    /// <summary>
    /// Checks if this line intersects with another line when neither line is vertical.
    /// </summary>
    /// <param name="other">The other line to compare.</param>
    /// <returns>True if this line intersects or overlays the other line; otherwise, false.</returns>
    private readonly bool IntersectsGradient(Line other)
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
    private readonly bool IntersectsVertical(Line other)
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
    private readonly bool IntersectsBothVertical(Line other)
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
    public readonly Point Other(Point point) => EndPoints.First(p => p != point);
}
