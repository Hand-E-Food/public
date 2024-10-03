using System;
using System.Collections.Generic;
using System.Linq;

namespace Underground;

/// <summary>
/// A moment that branches into different options.
/// </summary>
/// <param name="randomSeed">This branch's random seed.</param>
public abstract class Branch(int randomSeed)
{
    private readonly int randomSeed = randomSeed;

    /// <summary>
    /// This branch's seeded random number generator.
    /// </summary>
    protected Random Random { get; } = new(randomSeed);

    /// <summary>
    /// This branch's default outcome if there are no valid options.
    /// </summary>
    public virtual void DefaultOutcome()
    { }


    /// <summary>
    /// Chooses an option.
    /// </summary>
    /// <returns>The outcome of an option.</returns>
    public Outcome GetOutcome()
    {
        List<BranchOption> options = WeighOptions().Where(option => option.Weight > 0).ToList();
        if (options.Count == 0) return DefaultOutcome;
        int n = Random.Next(options.Sum(option => option.Weight));
        foreach (BranchOption option in options)
        {
            n -= option.Weight;
            if (n < 0) return option.Outcome;
        }
        throw new InvalidOperationException("Randomly chose an option that doesn't exist.");
    }

    /// <summary>
    /// Generates the weighted options to choose from.
    /// </summary>
    /// <returns>A list of weighted options to choose from.</returns>
    protected abstract IEnumerable<BranchOption> WeighOptions();
}
