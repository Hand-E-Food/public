using System.Diagnostics.CodeAnalysis;

namespace Bots.Models;
public readonly struct RelativeVector(int linear, int lateral) : IEquatable<RelativeVector>
{
    public readonly int Linear = linear;
    public readonly int Lateral = lateral;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is RelativeVector other && Equals(other);
    public bool Equals(RelativeVector other) => Linear == other.Linear && Lateral == other.Lateral;
    public override int GetHashCode() => HashCode.Combine(Linear, Lateral);

    public static bool operator ==(RelativeVector a, RelativeVector b) => a.Equals(b);
    public static bool operator !=(RelativeVector a, RelativeVector b) => !(a == b);
    public static RelativeVector operator +(RelativeVector a, RelativeVector b) => new(a.Linear + b.Linear, a.Lateral + b.Lateral);
    public static RelativeVector operator -(RelativeVector a, RelativeVector b) => new(a.Linear - b.Linear, a.Lateral - b.Lateral);
    public static RelativeVector operator *(RelativeVector a, int b) => new(a.Linear * b, a.Lateral * b);

    public static readonly RelativeVector Zero = new(0, 0);
}
