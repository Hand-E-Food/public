using PsiFi.Geometry;
using PsiFi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PsiFi
{
    class Random : System.Random
    {
        /// <inheritdoc/>
        public Random() : base() { }

        /// <inheritdoc/>
        public Random(int seed) : base(seed) { }

        /// <summary>
        /// Returns a random element from the <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to get an element from.</param>
        /// <returns>A random element from the <paramref name="collection"/>.</returns>
        public T Next<T>(ICollection<T> collection)
        {
            var n = Next(collection.Count);
            return collection.Skip(n).First();
        }

        /// <summary>
        /// Returns the result of applying random damage.
        /// </summary>
        /// <param name="damageRange">The damage to apply.</param>
        /// <returns>An absolute damage value.</returns>
        public Damage Next(DamageRange damageRange)
        {
            return new Damage(Next(damageRange.Dice));
        }

        /// <summary>
        /// Returns the result of rolling a collection of dice.
        /// </summary>
        /// <param name="dice">The dice to roll.</param>
        /// <returns>The dices' total result.</returns>
        public int Next(Dice dice) => dice.Roll(this);

        /// <summary>
        /// Shuffles a collection of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the collection.</typeparam>
        /// <param name="collection">The collection to shuffle.</param>
        /// <returns>A shuffled list containing all elements from the collection.</returns>
        public List<T> Shuffle<T>(IEnumerable<T> collection)
        {
            var candidates = collection.ToList();
            var shuffled = new List<T>(candidates.Count);
            while (candidates.Count > 0)
            {
                var item = Next(candidates);
                candidates.Remove(item);
                shuffled.Add(item);
            }
            return shuffled;
        }
    }
}
