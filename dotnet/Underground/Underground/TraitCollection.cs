using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Underground;

/// <summary>
/// A collection of traits with varying values.
/// </summary>
public class TraitCollection : IEnumerable<KeyValuePair<Trait, int>>
{
    private readonly Dictionary<Trait, int> traits = [];

    /// <summary>
    /// The value of the specified trait.
    /// </summary>
    /// <param name="trait">The trait.</param>
    public int this[Trait trait]
    {
        get => traits.GetValueOrDefault(trait, 0);
        set
        {
            if (value == 0)
                traits.Remove(trait);
            else
                traits[trait] = value;
        }
    }

    /// <summary>
    /// Add the specified number of trait levels.
    /// </summary>
    /// <param name="trait">The trait.</param>
    /// <param name="value">The value to add to the trait.</param>
    public void Add(Trait trait, int value) => this[trait] += value;

    public IEnumerator<KeyValuePair<Trait, int>> GetEnumerator() => traits.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// The total number of levels in this collection.
    /// </summary>
    public int TotalLevels => traits.Sum(pair => pair.Value);

    /// <summary>
    /// Adds the values of the traits in each collection.
    /// </summary>
    /// <param name="a">A <see cref="TraitCollection"/>.</param>
    /// <param name="b">A <see cref="TraitCollection"/>.</param>
    /// <returns>A <see cref="TraitCollection"/> with the sum of the traits in each collection.</returns>
    public static TraitCollection operator +(TraitCollection a, TraitCollection b)
    {
        TraitCollection result = [];
        TraitCollection[] traitCollections = [a, b];
        foreach (TraitCollection traitCollection in traitCollections)
            foreach ((Trait trait, int value) in traitCollection)
                result.Add(trait, value);
        return result;
    }

    /// <summary>
    /// Negates the values of the traits.
    /// </summary>
    /// <param name="a">A <see cref="TraitCollection"/>.</param>
    /// <returns>A <see cref="TraitCollection"/> with the traits values negated.</returns>
    public static TraitCollection operator -(TraitCollection a)
    {
        TraitCollection result = [];
        foreach ((Trait trait, int value) in a)
            result.Add(trait, -value);
        return result;
    }

    /// <summary>
    /// Subtracts the values of the traits in second collection to those in the first collection.
    /// </summary>
    /// <param name="a">The initial <see cref="TraitCollection"/>.</param>
    /// <param name="b">The <see cref="TraitCollection"/> to subtract.</param>
    /// <returns>A <see cref="TraitCollection"/> with the sum of the traits in each collection.</returns>
    public static TraitCollection operator -(TraitCollection a, TraitCollection b)
        {
        TraitCollection result = [];
        foreach ((Trait trait, int value) in a)
            result.Add(trait, value);
        foreach ((Trait trait, int value) in b)
            result.Add(trait, -value);
        return result;
    }

    /// <summary>
    /// Multiplies the values of the matching traits in each collection. Traits that only appear in one collection do
    /// not appear in the result.
    /// </summary>
    /// <param name="a">A <see cref="TraitCollection"/>.</param>
    /// <param name="b">A <see cref="TraitCollection"/>.</param>
    /// <returns>A <see cref="TraitCollection"/> with the product of the matching traits in each collection.</returns>
    public static TraitCollection operator *(TraitCollection a, TraitCollection b)
    {
        TraitCollection result = [];
        foreach ((Trait trait, int value) in a)
            result.Add(trait, value * b[trait]);
        return result;
    }
}
