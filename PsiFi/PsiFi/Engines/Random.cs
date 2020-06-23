using System.Collections.Generic;

namespace PsiFi.Engines
{
    class Random : IRandom
    {
        private readonly System.Random random;

        public Random()
        {
            random = new System.Random();
        }

        public Random(int seed)
        {
            random = new System.Random(seed);
        }

        /// <inheritdoc/>
        public int Next(int maxValue) => random.Next(maxValue);

        /// <inheritdoc/>
        public int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);

        /// <inheritdoc/>
        public int Next(Range range) => random.Next(range.Minimum, range.Maximum + 1);

        /// <inheritdoc/>
        public T Next<T>(IList<T> collection) => collection[random.Next(collection.Count)];
    }
}
