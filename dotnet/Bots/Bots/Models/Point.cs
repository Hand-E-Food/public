using System.Diagnostics.CodeAnalysis;

namespace Bots.Models;
public readonly struct Point(int x, int y) : IEquatable<Point>
{
    public readonly int X = x;
    public readonly int Y = y;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point other && Equals(other);

    public bool Equals(Point other) => X == other.X && Y == other.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Point a, Point b) => a.Equals(b);
    public static bool operator !=(Point a, Point b) => !(a == b);
    public static Point operator +(Point p, Vector v) => new(p.X + v.DX, p.Y + v.DY);
    public static Point operator -(Point p, Vector v) => new(p.X - v.DX, p.Y - v.DY);
    public static Vector operator -(Point a, Point b) => new(b.X - a.X, b.Y - a.Y);
}
