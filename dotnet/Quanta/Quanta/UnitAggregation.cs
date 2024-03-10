using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quanta
{
    public class UnitAggregation : Aggregation<Unit>
    {

        public DimensionAggregation Dimensions
        {
            get
            {
                var result = new Dictionary<Dimension, int>();

                foreach (var item in Items)
                {
                    var dimension = item.Key.Dimension;
                    int value;
                    if (result.TryGetValue(dimension, out value))
                    {
                        value += item.Value;
                        if (value == 0)
                            result.Remove(dimension);
                        else
                            result[dimension] = value;
                    }
                    else
                    {
                        result.Add(dimension, item.Value);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// The ratio of this aggregation of units to the SI units.
        /// </summary>
        public double RatioToSI => Items.Any()
            ? Items
                .Select(x => Math.Pow(x.Key.RatioToSI, x.Value))
                .Aggregate((a, b) => a * b)
            : 1.0;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitAggregation"/> class with no units.
        /// </summary>
        public UnitAggregation()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitAggregation"/> class.
        /// </summary>
        /// <param name="unit">The single unit to add to this <see cref="UnitAggregation"/>.</param>
        /// <param name="power">The unit's power.</param>
        public UnitAggregation(Unit unit, int power = 1)
        {
            Add(unit, power);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitAggregation"/> class.
        /// </summary>
        /// <param name="units">The units to add to this <see cref="UnitAggregation"/> and the power of each.</param>
        public UnitAggregation(IEnumerable<KeyValuePair<Unit, int>> units)
        {
            foreach (var unit in units)
                Add(unit.Key, unit.Value);
        }

        public new UnitAggregation Clone() => (UnitAggregation)base.Clone();

        public override string ToString()
        {
            var result = new StringBuilder();
            var sortedItems = Items
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key.Dimension.Name)
                .ThenBy(x => x.Key.Abbreviation);

            foreach (var item in sortedItems)
                result.AppendFormat(item.Value == 1 ? "·{0}" : "·{0}^{1}", item.Key.Abbreviation, item.Value);

            return result.ToString();
        }

        public static UnitAggregation operator *(UnitAggregation a, UnitAggregation b) =>
            (UnitAggregation)Multiply(a, b);

        public static UnitAggregation operator /(UnitAggregation a, UnitAggregation b) =>
            (UnitAggregation)Divide(a, b);
    }
}
