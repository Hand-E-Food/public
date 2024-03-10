namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Formats a <see cref="Range"/> object.
    /// </summary>
    internal abstract class RangeFormat
    {
        /// <summary>
        /// Gets the minimum width required to display this format.
        /// </summary>
        /// <param name="range">The range to display.</param>
        /// <returns>The width required to draw this range in this format.</returns>
        public abstract int MinimumWidth(Range range);

        /// <summary>
        /// Formats the specified range.
        /// </summary>
        /// <param name="range">The range to format.</param>
        /// <returns>The formatted string.</returns>
        public abstract string Format(Range range);

        /// <summary>
        /// Displays a percentage of the range's value over its maximum.
        /// </summary>
        /// <remarks>
        /// If the range's maximum is not positive, this formats the range as "0%".
        /// </remarks>
        public static readonly RangeFormat Fraction = new FractionRangeFormat();
        private class FractionRangeFormat : RangeFormat
        {
            public override int MinimumWidth(Range range) =>
                range.Maximum.ToString().Length * 2 + 1;
            public override string Format(Range range) =>
                $"{range.Value}/{range.Maximum}".PadLeft(MinimumWidth(range));
        }

        /// <summary>
        /// Displays the range's value, minimum and maximum.
        /// </summary>
        public static readonly RangeFormat FullRange = new FullRangeFormat();
        private class FullRangeFormat : RangeFormat
        {
            public override int MinimumWidth(Range range) =>
                range.Maximum.ToString().Length * 2 + range.Minimum.ToString().Length + 4;
            public override string Format(Range range) =>
                $"{range.Value} ({range.Minimum}-{range.Maximum})".PadLeft(MinimumWidth(range));
        }

        /// <summary>
        /// Displays a static string as an overflow value.
        /// </summary>
        /// <param name="value">The string to display.</param>
        public static RangeFormat Overflow(string value) => new OverflowRangeFormat(value);
        private class OverflowRangeFormat : RangeFormat
        {
            private readonly string value;
            public OverflowRangeFormat(string value) => this.value = value;
            public override int MinimumWidth(Range range) => 0;
            public override string Format(Range range) => value;
        }

        /// <summary>
        /// Displays a percentage of the range's value over its maximum.
        /// </summary>
        /// <remarks>
        /// If the range's maximum is not positive, this formats the range as "0%".
        /// </remarks>
        public static readonly RangeFormat Percentage = new PercentageRangeFormat();
        private class PercentageRangeFormat : RangeFormat
        {
            public override int MinimumWidth(Range range) => 4;
            public override string Format(Range range)
            {
                var percentage = range.Maximum > 0
                    ? 100 * range.Value / range.Maximum
                    : 0;
                return $"{percentage:n0,3}%";
            }
        }

        /// <inheritdoc cref="Pips(char, char, char)"/>
        public static RangeFormat Pips(char pip) => new PipsRangeFormat(null, pip, null);
        /// <inheritdoc cref="Pips(char, char, char)"/>
        public static RangeFormat Pips(char pip, char maximum) => new PipsRangeFormat(pip, pip, maximum);
        /// <summary>
        /// Displays the range as a sequence of pips.
        /// </summary>
        /// <param name="minimum">The character to use for pip 1 up to the range's minimum.</param>
        /// <param name="pip">The character to use as a pip up to the range's value.</param>
        /// <param name="maximum">The character to use for pips between the range's value and maximum.</param>
        public static RangeFormat Pips(char minimum, char pip, char maximum) => new PipsRangeFormat(minimum, pip, maximum);
        private class PipsRangeFormat : RangeFormat
        {
            private char minimum;
            private char value;
            private char maximum;
            public PipsRangeFormat(char? minimum, char value, char? maximum)
            {
                this.minimum = minimum ?? value;
                this.value = value;
                this.maximum = maximum ?? '·';
            }
            public override int MinimumWidth(Range range) => range.Maximum;
            public override string Format(Range range) =>
                new string(minimum, range.Minimum).PadRight(range.Value, value).PadRight(range.Maximum, maximum);
        }

        /// <summary>
        /// Displays only the range's value.
        /// </summary>
        public static readonly RangeFormat Value = new ValueRangeFormat();
        private class ValueRangeFormat : RangeFormat
        {
            public override int MinimumWidth(Range range) => range.Value.ToString().Length;
            public override string Format(Range range) => range.Value.ToString();
        }
    }
}
