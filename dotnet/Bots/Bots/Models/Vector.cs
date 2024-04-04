using System.Diagnostics.CodeAnalysis;

namespace Bots.Models;
public readonly struct Vector(int dx, int dy) : IEquatable<Vector>
{
    public readonly int DX = dx;
    public readonly int DY = dy;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector other && Equals(other);
    public bool Equals(Vector other) => DX == other.DX && DY == other.DY;
    public override int GetHashCode() => HashCode.Combine(DX, DY);

    public static bool operator ==(Vector a, Vector b) => a.Equals(b);
    public static bool operator !=(Vector a, Vector b) => !(a == b);
    public static Vector operator +(Vector a, Vector b) => new(a.DX + b.DX, a.DY + b.DY);
    public static Vector operator -(Vector a, Vector b) => new(a.DX - b.DX, a.DY - b.DY);
    public static Vector operator *(Vector a, int b) => new(a.DX *b, a.DY *b);

    public Vector Rotate180() => new(-DX, -DY);
    public Vector RotateLeft90() => new(DY, -DX);
    public Vector RotateRight90() => new(-DY, DX);

    public static readonly Vector East = new(1, 0);
    public static readonly Vector North = new(0, -1);
    public static readonly Vector South = new(0, 1);
    public static readonly Vector West = new(-1, 0);
    public static readonly Vector Zero = new(0, 0);
}
