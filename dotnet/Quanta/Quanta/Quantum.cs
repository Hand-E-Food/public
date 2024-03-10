using System;
using System.Linq;
using System.Text;

namespace Quanta
{
    public class Quantum
    {
        /// <summary>
        /// This quantum's magnitude.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// This quantum's units.
        /// </summary>
        public UnitAggregation Units { get; private set; }

        /// <summary>
        /// Initialises a new quantum with magnitude 0 and no Quanta.
        /// </summary>
        public Quantum()
            : this(0.0)
        { }

        /// <summary>
        /// Initialises a new quantum with the specified magnitude and no dimension.
        /// </summary>
        /// <param name="value">The quantum's magnitude.</param>
        public Quantum(double value)
        {
            Value = value;
            Units = new UnitAggregation();
        }

        /// <summary>
        /// Initialises a new quantum with the specified magnitude and dimension.
        /// </summary>
        /// <param name="value">The quantum's magnitude.</param>
        /// <param name="dimension">The quantum's dimension.</param>
        /// <param name="power">The dimension's power.</param>
        public Quantum(double value, Unit unit, int power = 1)
        {
            Value = value;
            Units = new UnitAggregation(unit, power);
        }

        /// <summary>
        /// Initialises a new quantum of the specified value and dimension.
        /// </summary>
        /// <param name="value">The quantum's magnitude.</param>
        /// <param name="dimension">The quantum's dimension.</param>
        public Quantum(double value, UnitAggregation units)
        {
            Value = value;
            Units = units;
        }

        public Quantum ChangeUnits(UnitAggregation newUnits)
        {
            

            var value = Value / Units.RatioToSI * newUnits.RatioToSI;
            return new Quantum(value, newUnits);
        }

        public override bool Equals(object obj) => this == obj as Quantum;

        public override int GetHashCode() => Value.GetHashCode() ^ Units.GetHashCode();

        public override string ToString() => Value.ToString() + Units.ToString();

        public string ToString(string format) => Value.ToString(format) + Units.ToString();

        public static bool operator ==(Quantum a, Quantum b)
        {
            return (
                    (object)a == null
                    && (object)b == null
                )
                || (
                    (object)a != null
                    && (object)b != null
                    && a.Value == b.Value
                    && a.Units == b.Units
                );
        }

        public static bool operator !=(Quantum a, Quantum b) => !(a == b);

        public static bool operator <(Quantum a, Quantum b)
        {
            return (object)a != null
                && (object)b != null
                && a.Value < b.Value
                && a.Units == b.Units;
        }

        public static bool operator >(Quantum a, Quantum b)
        {
            return (object)a != null
                && (object)b != null
                && a.Value > b.Value
                && a.Units == b.Units;
        }

        public static bool operator <=(Quantum a, Quantum b)
        {
            return (
                    (object)a == null
                    && (object)b == null
                )
                || (
                    (object)a != null
                    && (object)b != null
                    && a.Value <= b.Value
                    && a.Units == b.Units
                );
        }

        public static bool operator >=(Quantum a, Quantum b)
        {
            return (
                    (object)a == null
                    && (object)b == null
                )
                || (
                    (object)a != null
                    && (object)b != null
                    && a.Value >= b.Value
                    && a.Units == b.Units
                );
        }

        public static Quantum operator +(Quantum a, Quantum b)
        {
            if (a.Units != b.Units)
                throw new InvalidOperationException("Mismatched units.");
            else
                return new Quantum(a.Value + b.Value, a.Units);
        }

        public static Quantum operator -(Quantum a, Quantum b)
        {
            if (a.Units != b.Units)
                throw new InvalidOperationException("Mismatched units.");
            else
                return new Quantum(a.Value - b.Value, a.Units);
        }

        public static Quantum operator *(Quantum a, Quantum b) => new Quantum(a.Value * b.Value, a.Units * b.Units);

        public static Quantum operator /(Quantum a, Quantum b) => new Quantum(a.Value / b.Value, a.Units / b.Units);
    }
}
