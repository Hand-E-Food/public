using System.Collections;

namespace RandomVectorMap;

/// <summary>
/// A set of items that are to be selected randomly but with weight given to each item.
/// </summary>
/// <typeparam name="T">The type of items to store.</typeparam>
public class WeightedRandomSet<T> : IEnumerable<KeyValuePair<T, double>> where T : notnull
{
    /// <summary>
    /// Gets or sets the weight of an item.
    /// </summary>
    /// <param name="item">The item to get or set the weight of.</param>
    /// <value>The weight of the item.</value>
    public double this[T item]
    {
        get => items.TryGetValue(item, out double value) ? value : 0;
        set
        {
            if (value != 0)
                items[item] = value;
            else
                items.Remove(item);

            CalculateTotalWeight();
        }
    }

    /// <summary>
    /// The collection of items.
    /// </summary>
    private readonly Dictionary<T, double> items = [];

    public void Add(T key, int value)
    {
        items.Add(key, value);
        CalculateTotalWeight();
    }

    /// <summary>
    /// Gets or sets the collection's total weight.
    /// </summary>
    public double TotalWeight { get; private set; } = 0;

    /// <summary>
    /// Gets the list of values stored in this table.
    /// </summary>
    /// <value>The list of values stored in this table.</value>
    public IEnumerable<T> Values => items.Select(item => item.Key);

    /// <summary>
    /// Calculates and sets the value for TotalWeight.
    /// </summary>
    /// <remarks>It is more reliable to recalculate the value each time than rely on sequential addition
    /// and subtraction with floating point numbers.</remarks>
    private void CalculateTotalWeight()
    {
        TotalWeight = items.Sum(item => item.Value);
    }

    public IEnumerator<KeyValuePair<T, double>> GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
}
