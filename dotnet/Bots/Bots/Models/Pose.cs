using System.Diagnostics.CodeAnalysis;

namespace Bots.Models;
public readonly struct Pose(Point point, Vector vector) : IEquatable<Pose>
{
    public readonly Point Location = point;
    public readonly Vector Orientation = vector;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Pose other && Equals(other);
    public bool Equals(Pose other) => Location == other.Location && Orientation == other.Orientation;
    public override int GetHashCode() => HashCode.Combine(Location.X, Location.Y, Orientation.DX, Orientation.DY);

    public static bool operator ==(Pose a, Pose b) => a.Equals(b);
    public static bool operator !=(Pose a, Pose b) => !(a == b);
    public static Point operator +(Pose p, RelativeVector v) => p.Location + p.Orientation * v.Linear + p.Orientation.RotateRight90() * v.Lateral;

    public static readonly Pose Empty = new();
}
