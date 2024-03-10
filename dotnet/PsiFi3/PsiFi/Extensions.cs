namespace PsiFi
{
    internal static class Extensions
    {
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> sources) => sources.SelectMany(source => source);

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) => source.Where(item => item != null)!;
    }
}
