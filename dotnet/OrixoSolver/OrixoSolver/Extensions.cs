using System.Collections.Generic;

namespace OrixoSolver
{
    public static class Extensions
    {
        public static bool HasOnly<T>(this IEnumerable<T> source, T value)
        {
            var enumerator = source.GetEnumerator();
            return enumerator.MoveNext()
                && enumerator.Current.Equals(value)
                && !enumerator.MoveNext();
        }

        public static bool TrySingle<T>(this IEnumerable<T> source, out T value)
        {
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                value = default;
                return false;
            }
            value = enumerator.Current;
            return !enumerator.MoveNext();
        }
    }
}
