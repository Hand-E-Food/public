using System.Collections.Generic;

namespace Underground;

/// <summary>
/// Details accumulated levels of a trait.
/// </summary>
/// <param name="trait">The trait.</param>
/// <param name="level">The trait's level.</param>
public class TraitLevel(Trait trait, int level)
{
    /// <summary>
    /// The trait.
    /// </summary>
    public Trait Trait { get; } = trait;

    /// <summary>
    /// The trait's level.
    /// </summary>
    public int Level { get; set; } = level;

    public static implicit operator TraitLevel(KeyValuePair<Trait, int> value) => new(value.Key, value.Value);
    public static implicit operator KeyValuePair<Trait, int>(TraitLevel value) => new(value.Trait, value.Level);
}
