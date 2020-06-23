using System;

namespace PsiFi
{
    class Range
    {
        public int Maximum { get; private set; }
        public int Minimum { get; private set; }
        public int Value
        {
            get => value;
            set
            {
                if (value < Minimum)
                    this.value = Minimum;
                else if (value > Maximum)
                    this.value = Maximum;
                else
                    this.value = value;
                OnChanged();
            }
        }
        private int value;

        public event EventHandler Changed;
        protected virtual void OnChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public Range(int maximum)
        {
            value = maximum;
            SetRange(0, maximum);
        }

        public Range(int minimum, int maximum)
        {
            value = maximum;
            SetRange(minimum, maximum);
        }

        public Range(int minimum, int maximum, int value)
        {
            this.value = value;
            SetRange(minimum, maximum);
        }

        public void SetRange(int minimum, int maximum)
        {
            if (minimum > maximum) throw new ArgumentOutOfRangeException(nameof(maximum), $"{nameof(maximum)} cannot be less than {nameof(minimum)}.");
            Minimum = minimum;
            Maximum = maximum;
            if (value < Minimum)
                value = Minimum;
            else if (value > Maximum)
                value = Maximum;
            OnChanged();
        }
    }
}
