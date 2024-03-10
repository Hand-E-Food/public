using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quanta
{
    public abstract class Aggregation<T> : ICloneable, IEnumerable<KeyValuePair<T, int>>
    {

        protected Dictionary<T, int> Items { get; } = new Dictionary<T, int>();

        protected void Add(T item, int power)
        {
            if (power == 0)
                return;

            int value;
            if (Items.TryGetValue(item, out value))
            {
                power += value;
                if (power == 0)
                    Items.Remove(item);
                else
                    Items[item] = power;
            }
            else
            {
                Items.Add(item, power);
            }
        }

        public Aggregation<T> Clone()
        {
            var result = (Aggregation<T>)Activator.CreateInstance(GetType());
            foreach (var item in Items)
                result.Items.Add(item.Key, item.Value);
            return result;
        }
        object ICloneable.Clone() => Clone();

        public override bool Equals(object obj)
        {
            var other = obj as Aggregation<T>;
            if (other == null || this.Items.Count != other.Items.Count)
                return false;

            int value;
            foreach (var item in Items)
                if (!other.Items.TryGetValue(item.Key, out value) || value != item.Value)
                    return false;

            return true;
        }

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override int GetHashCode() => Items
            .Select(x => x.Key.GetHashCode() * x.Value)
            .Aggregate((a, b) => a ^ b);

        public static Aggregation<T> Multiply(Aggregation<T> a, Aggregation<T> b)
        {
            var result = a.Clone();
            foreach (var item in b.Items)
                result.Add(item.Key, item.Value);
            return result;
        }

        public static Aggregation<T> Divide(Aggregation<T> a, Aggregation<T> b)
        {
            var result = a.Clone();
            foreach (var item in b.Items)
                result.Add(item.Key, -item.Value);
            return result;
        }
    }
}
