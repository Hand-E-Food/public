namespace RandomVectorMap;

public static class RandomExtensions
{
    /// <summary>
    /// Returns a random element from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the list.</typeparam>
    /// <param name="random">The random number generator to use.</param>
    /// <param name="list">The list to select from.</param>
    /// <returns>A random element from the list.</returns>
    /// <exception cref="ArgumentException"><paramref name="list"/> is empty.</exception>
    public static T Next<T>(this Random random, IList<T> list)
    {
        if (list.Count == 0) throw new ArgumentException($"{nameof(list)} is empty.");
        int index = random.Next(list.Count);
        return list[index];
    }

    /// <summary>
    /// Returns a random element from the weighted set.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the set.</typeparam>
    /// <param name="random">The random number generator to use.</param>
    /// <param name="list">The weighted set to select from.</param>
    /// <returns>A random element from the weighted set.</returns>
    /// <exception cref="ArgumentException"><paramref name="list"/> is empty.</exception>
    public static T Next<T>(this Random random, WeightedRandomSet<T> list) where T : notnull
    {
        double weightCount = random.NextDouble() * list.TotalWeight;
        foreach (var item in list)
        {
            weightCount -= item.Value;
            if (weightCount < 0) return item.Key;
        }
        // Likely a rounding error. Just return the last item.
        return list.Last().Key;
    }

    /// <summary>
    /// Returns a random element and removes it from the list.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the list.</typeparam>
    /// <param name="random">The random number generator to use.</param>
    /// <param name="list">The list to select from. The list will be modified.</param>
    /// <returns>A random element from the list.</returns>
    /// <exception cref="ArgumentException"><paramref name="list"/> is empty.</exception>
    public static T Pop<T>(this Random random, List<T> list)
    {
        if (list.Count == 0) throw new ArgumentException($"{nameof(list)} is empty.");
        int index = random.Next(list.Count);
        var result = list[index];
        list.RemoveAt(index);
        return result;
    }
}
