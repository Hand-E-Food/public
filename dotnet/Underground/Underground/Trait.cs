using System;
using System.Runtime.CompilerServices;

namespace Underground;

public class Trait : IEquatable<Trait>
{
    public static readonly Trait Bluffing = new();
    public static readonly Trait Combat = new();

    private Trait([CallerMemberName]string? name = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        this.name = name;
    }
    private readonly string name;
    public override bool Equals(object? obj) => Equals(obj as Trait);
    public bool Equals(Trait? other) => other is not null && this == other;
    public override int GetHashCode() => name.GetHashCode();
    public override string ToString() => name;
    public static bool operator ==(Trait a, Trait b) => a.name == b.name;
    public static bool operator !=(Trait a, Trait b) => !(a == b);
}