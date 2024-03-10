using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanta
{
    public class DimensionAggregation : Aggregation<Dimension>
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="DimensionAggregation"/> class with no dimensions.
        /// </summary>
        public DimensionAggregation()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DimensionAggregation"/> class.
        /// </summary>
        /// <param name="units">The dimensions to add to this <see cref="DimensionAggregation"/> and the power of each.</param>
        public DimensionAggregation(IEnumerable<KeyValuePair<Unit, int>> dimensions)
        {
            foreach (var dimension in dimensions)
                Add(dimension.Key.Dimension, dimension.Value);
        }

        public new DimensionAggregation Clone() => (DimensionAggregation)base.Clone();

        public override string ToString()
        {
            var result = new StringBuilder();
            var sortedItems = Items
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Key.Name);

            foreach (var item in sortedItems)
                result.AppendFormat(item.Value == 1 ? "·{0}" : "·{0}^{1}", item.Key.Name, item.Value);

            return result.ToString();
        }

        public static DimensionAggregation operator *(DimensionAggregation a, DimensionAggregation b) =>
            (DimensionAggregation)Multiply(a, b);

        public static DimensionAggregation operator /(DimensionAggregation a, DimensionAggregation b) =>
            (DimensionAggregation)Divide(a, b);
    }
}
