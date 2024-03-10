using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quanta
{
    public class UnitCollection : ICollection<Unit>
    {
        /// <summary>
        /// Gets the number of elements contained in the <see cref="UnitCollection"/>.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// true if the <see cref="UnitCollection"/> is read-only; otherwise, false.
        /// </summary>
        public bool IsReadOnly => false;

        private Dictionary<string, Unit> Items { get; } = new Dictionary<string, Unit>();

        public Unit SIUnit { get; private set; }

        public Unit this[string abbreviation] => Items[abbreviation];

        /// <summary>
        /// Adds a <see cref="Unit"/> to the <see cref="UnitCollection"/>.
        /// </summary>
        /// <param name="unit">The <see cref="Unit"/> to add to the <see cref="UnitCollection"/>.</param>
        public void Add(Unit unit)
        {
            Items.Add(unit.Abbreviation, unit);
            if (unit.RatioToSI == 1)
                SIUnit = unit;
        }

        public void Clear() => Items.Clear();

        public bool Contains(string abbreviation) => Items.ContainsKey(abbreviation);

        public bool Contains(Unit unit) => Items.ContainsValue(unit);

        public void CopyTo(Unit[] array, int arrayIndex) => Items.Values.CopyTo(array, arrayIndex);

        public IEnumerator<Unit> GetEnumerator() => Items.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Remove(string abbreviation) => Items.Remove(abbreviation);

        public bool Remove(Unit unit) => Items.Remove(unit.Abbreviation);
    }
}
