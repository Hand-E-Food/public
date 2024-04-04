namespace RandomVectorMap;

public static class EnumerableExtensions
{
    /// <summary>
    /// Throws an exception if any element in the collection is null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The collection to scan.</param>
    /// <returns>The elements as a non-nullable <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentException">An element of the collection is null.</exception>
    public static IEnumerable<T> AssertNotNull<T>(this IEnumerable<T?> source)
    {
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current is null)
                throw new ArgumentException($"{nameof(source)} contains a null element.");
            yield return enumerator.Current;
        }
    }

    /// <summary>
    /// Filters a collection, returning only its non-null elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="source">The collection to scan.</param>
    /// <returns>The non-null elements of <paramref name="source"/>.</returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
    {
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            if (enumerator.Current is not null)
                yield return enumerator.Current;
    }
}
