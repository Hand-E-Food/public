using System;
using System.Collections.Generic;

namespace Rogue
{
    public static class EnumerableExtensions
    {

        public static T FirstWithLeast<T,U>(this IEnumerable<T> source, Func<T, U> selector) where U : IComparable<U>
        {
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException($"{nameof(source)} is empty", nameof(source));

            var best = enumerator.Current;
            var bestScore = selector(best);

            while (enumerator.MoveNext())
            {
                var currentScore = selector(enumerator.Current);
                if (currentScore.CompareTo(bestScore) < 0)
                {
                    best = enumerator.Current;
                    bestScore = currentScore;
                }
            }
            return best;
        }

        public static T FirstWithMost<T, U>(this IEnumerable<T> source, Func<T, U> selector) where U : IComparable<U>
        {
            var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentException($"{nameof(source)} is empty", nameof(source));

            var best = enumerator.Current;
            var bestScore = selector(best);

            while (enumerator.MoveNext())
            {
                var currentScore = selector(enumerator.Current);
                if (currentScore.CompareTo(bestScore) > 0)
                {
                    best = enumerator.Current;
                    bestScore = currentScore;
                }
            }
            return best;
        }
    }
}
