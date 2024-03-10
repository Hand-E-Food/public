using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomVectorMap
{

    /// <summary>
    /// A set of items that are to be selected randomly but with weight given to each item.
    /// </summary>
    /// <typeparam name="T">The type of items to store.</typeparam>
    public class WeightedRandomSet<T>
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="WeightedSet<T>"/> class.
        /// </summary>
        /// <param name="random">The random number generator to use.</param>
        public WeightedRandomSet()
        {
            Random = new Random();
            TotalWeight = 0;
        }

        #region Properties ...

        /// <summary>
        /// Gets or sets the weight of an item.
        /// </summary>
        /// <param name="item">The item to get or set the weight of.</param>
        /// <value>The weight of the item.</value>
        public double this[T item]
        {
            get 
            { 
                if (items.ContainsKey(item))
                    return items[item];
                else
                    return 0;
            }
            set 
            {
                if (value != 0)
                {
                    if (items.ContainsKey(item))
                    {
                        items[item] = value; 
                    }
                    else
                    {
                        items.Add(item, value);
                    }
                }
                else
                {
                    if (items.ContainsKey(item))
                    {
                        items.Remove(item);
                    }
                }
                CalculateTotalWeight();
            }
        }

        /// <summary>
        /// The collection of items.
        /// </summary>
        private Dictionary<T, double> items = new Dictionary<T, double>();

        /// <summary>
        /// Gets or sets the random number generator to use.
        /// </summary>
        /// <value>A random number generator.</value>
        public Random Random { get; set; }

        /// <summary>
        /// Gets or sets the collection's total weight.
        /// </summary>
        public double TotalWeight { get; private set; }

        /// <summary>
        /// Gets the list of values stored in this table.
        /// </summary>
        /// <value>The list of values stored in this table.</value>
        public IEnumerable<T> Values
        {
            get { return items.Select((item) => item.Key); }
        }

        #endregion

        /// <summary>
        /// Calculates and sets the value for TotalWeight.
        /// </summary>
        /// <remarks>It is more reliable to recalculate the value each time than rely on sequential addition
        /// and subtraction with floating point numbers.</remarks>
        private void CalculateTotalWeight()
        {
            TotalWeight = items.Sum((item) => item.Value);
        }

        /// <summary>
        /// Selects a random item from the collection.
        /// </summary>
        /// <returns>A randomly selected item.</returns>
        public T Select()
        {
            double weightCount = Random.NextDouble() * TotalWeight;
            foreach(var item in items)
            {
                weightCount -= item.Value;
                if (weightCount < 0) return item.Key;
            }
            // Likely a rounding error.  Just return the last item.
            return items.Last().Key;
        }
    }
}
