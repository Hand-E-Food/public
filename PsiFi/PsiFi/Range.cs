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

        public void SetRange(int minimum, int maximum)
        {
            if (minimum > maximum) throw new ArgumentOutOfRangeException(nameof(maximum), $"{nameof(maximum)} cannot be less than {nameof(minimum)}.");
            Minimum = minimum;
            Maximum = maximum;
        }
    }
}
