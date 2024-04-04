namespace RandomVectorMap.Mapping;

/// <summary>
/// Represents the difference between two points.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{DeltaX}, {DeltaY}, length {Length}")]
public readonly struct Vector
{
    /// <summary>
    /// Initialises a new instance of the <see cref="Vector"/> class.
    /// </summary>
    /// <param name="DeltaX">The vector's distance along the X axis.</param>
    /// <param name="DeltaY">The vecotr's distance along the Y axis.</param>
    public Vector(int deltaX, int deltaY) : this()
    {
        DeltaX = deltaX;
        DeltaY = deltaY;
        Gradient = (double)deltaY / (double)deltaX;
        Length = Math.Sqrt((double)deltaX * (double)deltaX + (double)deltaY * (double)deltaY);
    }
    /// <summary>
    /// Initialises a new instance of the <see cref="Vector"/> class.
    /// </summary>
    /// <param name="point1">The vector's starting point.</param>
    /// <param name="point2">The vecotr's ending point.</param>
    public Vector(Point point1, Point point2)
        : this(point2.X - point1.X, point2.Y - point1.Y)
    { }

    /// <summary>
    /// The distance along the X axis.
    /// </summary>
    public readonly int DeltaX;

    /// <summary>
    /// The distance along the Y axis.
    /// </summary>
    public readonly int DeltaY;

    /// <summary>
    /// Gets this line's gradient relative to the X axis.
    /// </summary>
    /// <value>This line's gradient relative to the X axis.</value>
    public readonly double Gradient;

    /// <summary>
    /// Gets the vector's length.
    /// </summary>
    /// <value>The vector's length.</value>
    public readonly double Length;

    /// <summary>
    /// Rotates this vector by 90°.
    /// </summary>
    /// <returns>This vector rotated by 90°.</returns>
    public readonly Vector Rotate90()
    {
        return new(-DeltaY, DeltaX);
    }

    /// <summary>
    /// Rotates this vector by 180°.
    /// </summary>
    /// <returns>This vector rotated by 180°.</returns>
    public readonly Vector Rotate180()
    {
        return new(-DeltaX, -DeltaY);
    }

    /// <summary>
    /// Rotates this vector by 270°.
    /// </summary>
    /// <returns>This vector rotated by 270°.</returns>
    public readonly Vector Rotate270()
    {
        return new(DeltaY, -DeltaX);
    }

    /// <summary>
    /// Adds two vectors together.
    /// </summary>
    /// <param name="point">The origin vector.</param>
    /// <param name="vector">The vector to add.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector operator +(Vector vector1, Vector vector2)
    {
        return new(vector1.DeltaX + vector2.DeltaX, vector1.DeltaY + vector2.DeltaY);
    }

    /// <summary>
    /// Adds the vector to a point.
    /// </summary>
    /// <param name="point">The origin point.</param>
    /// <param name="vector">The vector to add.</param>
    /// <returns>The resulting point.</returns>
    public static Point operator +(Point point, Vector vector)
    {
        point.Offset(vector.DeltaX, vector.DeltaY);
        return point;
    }

    /// <summary>
    /// Subtracts one vector from another.
    /// </summary>
    /// <param name="vector1">The original vector.</param>
    /// <param name="vector2">The vector to subtract.</param>
    /// <returns>The resulting vector.</returns>
    public static Vector operator -(Vector terminus, Vector origin)
    {
        return new(terminus.DeltaX - origin.DeltaX, terminus.DeltaY - origin.DeltaY);
    }

    /// <summary>
    /// Subtracts the vector from a point.
    /// </summary>
    /// <param name="point">The origin point.</param>
    /// <param name="vector">The vector to subtract.</param>
    /// <returns>The resulting point.</returns>
    public static Point operator -(Point point, Vector vector)
    {
        vector = vector.Rotate180();
        point.Offset(vector.DeltaX, vector.DeltaY);
        return point;
    }

    /// <summary>
    /// Scales this vector by a ratio.
    /// </summary>
    /// <param name="ratio">The ratio by which to scale this vector.</param>
    /// <returns>This vector scaled.</returns>
    public static Vector operator *(Vector vector, double ratio)
    {
        return new((int)(vector.DeltaX * ratio), (int)(vector.DeltaY * ratio));
    }

    /// <summary>
    /// Scales this vector by a ratio.
    /// </summary>
    /// <param name="ratio">The ratio by which to scale this vector.</param>
    /// <returns>This vector scaled.</returns>
    public static Vector operator /(Vector vector, double ratio)
    {
        return new((int)(vector.DeltaX / ratio), (int)(vector.DeltaY / ratio));
    }
}
