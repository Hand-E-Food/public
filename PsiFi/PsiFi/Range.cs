using System;

namespace PsiFi
{
    class Range
    {
        public int Maximum { get; private set; }
        public int Minimum { get; private set; }

        public Range(int maximum)
        {
            SetRange(0, maximum);
        }

        public Range(int minimum, int maximum)
        {
            SetRange(minimum, maximum);
        }

        /// <summary>
        /// Checks whether this <see cref="Range"/> contains the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// True if this <see cref="Range"/> contains the specified value.
        /// False if the <paramref name="value"/> falls outside of this <see cref="Range"/>.
        /// </returns>
        public bool Contains(int value) => Minimum <= value && value <= Maximum;

        public void SetRange(int minimum, int maximum)
        {
            if (minimum > maximum) throw new ArgumentOutOfRangeException(nameof(maximum), $"{nameof(maximum)} cannot be less than {nameof(minimum)}.");
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}
