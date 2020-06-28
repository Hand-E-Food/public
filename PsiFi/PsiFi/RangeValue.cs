using System;

namespace PsiFi
{
    class RangeValue : Range
    {
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

        public RangeValue(int maximum) : base(maximum)
        { }

        public RangeValue(int minimum, int maximum) : base(minimum, maximum)
        { }

        public RangeValue(int minimum, int maximum, int value) : base(minimum, maximum)
        {
            Value = value;
        }
    }
}
