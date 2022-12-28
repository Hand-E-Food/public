using System.Diagnostics;

namespace PsiFi
{
    /// <summary>
    /// A number constrained by an upper and lower bounds.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Range
    {
        internal string DebuggerDisplay => Minimum == 0
            ? $"{Value}/{Maximum}"
            : $"{Value} ({Minimum} to {Maximum})";

        /// <summary>
        /// This range's minimum value.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// This range's maximum value.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// This range's current value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Initialises a new <see cref="Range"/> with a minimum of 0 and the specified maximum.
        /// </summary>
        /// <param name="maximum">The range's maximum and initial value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximum"/> is negative.</exception>
        public Range(int maximum) : this(0, maximum) { }

        /// <summary>
        /// Initialises a new <see cref="Range"/> with the specified minimum and maximum.
        /// </summary>
        /// <param name="minimum">This range's minimum.</param>
        /// <param name="maximum">This range's maximum and initial value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximum"/> is less than <paramref name="minimum"/>.</exception>
        public Range(int minimum, int maximum)
        {
            if (minimum > maximum) throw new ArgumentOutOfRangeException(nameof(maximum), $"{nameof(maximum)} cannot be less than {nameof(minimum)}.");
            Minimum = minimum;
            Maximum = maximum;
            Value = maximum;
        }

        /// <summary>
        /// Ensures <see cref="Value"/> is between <see cref="Minimum"/> and <see cref="Maximum"/>.
        /// </summary>
        /// <returns>The amount <see cref="Value"/> was adjusted by.</returns>
        public int Constrain()
        {
            int oldValue = Value;
            Value = Math.Max(Minimum, Math.Min(Maximum, Value));
            return Value - oldValue;
        }

        /// <summary>
        /// Implicitly gets <paramref name="range"/>.<see cref="Value"/>.
        /// </summary>
        /// <param name="range">The range.</param>
        public static implicit operator int(Range range) => range.Value;
    }
}
