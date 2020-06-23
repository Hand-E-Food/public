using System.Collections;
using System.Collections.Generic;

namespace PsiFi.Engines
{
    interface IRandom
    {
        /// <summary>
        /// Gets a random number between 0 inclusive and <paramref name="max"/> exclusive.
        /// </summary>
        /// <param name="max">The exclusive maximum.</param>
        /// <returns>A random number.</returns>
        int Next(int max);

        /// <summary>
        /// Gets a random number between <paramref name="min"/> inclusive and <paramref name="max"/> exclusive.
        /// </summary>
        /// <param name="min">The inclusive minimum.</param>
        /// <param name="max">The exclusive maximum.</param>
        /// <returns>A random number.</returns>
        int Next(int min, int max);

        /// <summary>
        /// Gets a random number within the range's minimum and maximum, inclusive.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>A random number.</returns>
        int Next(Range range);

        /// <summary>
        /// Gets a random element from the <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>A random element from the <paramref name="collection"/>.</returns>
        T Next<T>(IList<T> collection);
    }
}
