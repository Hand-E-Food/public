using System.Diagnostics.CodeAnalysis;

namespace PsiFi
{
    /// <summary>
    /// A fraction that never loses precision, but has a limited denominator.
    /// </summary>
    public struct Fraction : IComparable<Fraction>, IComparable<int>, IEquatable<Fraction>
    {
        /// <summary>
        /// The maximum denominator supported by <see cref="Fraction"/>.
        /// </summary>
        public const int MaxDenominator = 10;

        /// <summary>
        /// The lowest common denominator for all denominators up to <see cref="MaxDenominator"/>.
        /// </summary>
        public const int Denominator = 1 * 2 * 3 * 2 * 5 * 1 * 7 * 2 * 3 * 1; //* 11 * 1 * 13 * 1 * 1 * 2 * 17 * 1 * 19 * 1;

        /// <summary>
        /// The maximum whole value supported by <see cref="Fraction"/>.
        /// </summary>
        public const int MaxValue = int.MaxValue / Denominator;

        /// <summary>
        /// The minimum whole value supported by <see cref="Fraction"/>.
        /// </summary>
        public const int MinValue = int.MinValue / Denominator;

        /// <summary>
        /// This fraction's numerator.
        /// </summary>
        public int Numerator;

        /// <summary>
        /// This fraction as a real number.
        /// </summary>
        public double AsDouble => (double)Numerator / Denominator;

        /// <summary>
        /// Creates a new <see cref="Fraction"/> with the lowest common denominator.
        /// </summary>
        /// <param name="numerator">This fraction's numerator.</param>
        private Fraction(int numerator)
        {
            Numerator = numerator;
        }

        /// <summary>
        /// Creates a new <see cref="Fraction"/>.
        /// </summary>
        /// <param name="numerator">The fraction's numerator.</param>
        /// <param name="denominator">The fraction's denominator.</param>
        /// <exception cref="ArgumentException">The denominator must be between 1 and
        /// <see cref="MaxDenominator"/>, positive or negative.</exception>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0 || denominator < -MaxDenominator || denominator > MaxDenominator)
                throw new ArgumentException($"{nameof(denominator)} must be betweeen 1 and {nameof(MaxDenominator)}, positive or negative.");
            Numerator = Denominator / denominator * numerator;
        }

        public int CompareTo(Fraction other) => Numerator.CompareTo(other.Numerator);
        public int CompareTo(int other)
        {
            if (other < MinValue)
                return 1;
            else if (other > MaxValue)
                return -1;
            else
                return Numerator.CompareTo(other * Denominator);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Fraction otherFraction)
                return Equals(otherFraction);
            else if (obj is int otherInt)
                return Equals(otherInt);
            else
                return false;
        }

        public bool Equals([NotNullWhen(true)] Fraction obj) => this == obj;

        public override int GetHashCode() => Numerator;

        public static implicit operator Fraction(int value) => new(value * Denominator);

        public static bool operator ==(Fraction left, Fraction right) => left.Numerator == right.Numerator;

        public static bool operator !=(Fraction left, Fraction right) => !(left == right);

        public static bool operator <(Fraction left, Fraction right) => left.Numerator < right.Numerator;

        public static bool operator >(Fraction left, Fraction right) => left.Numerator > right.Numerator;

        public static bool operator <=(Fraction left, Fraction right) => left.Numerator <= right.Numerator;

        public static bool operator >=(Fraction left, Fraction right) => left.Numerator >= right.Numerator;

        public static Fraction operator +(Fraction left, Fraction right) => new(left.Numerator + right.Numerator);

        public static Fraction operator -(Fraction left, Fraction right) => new(left.Numerator - right.Numerator);

        public static Fraction operator *(Fraction left, Fraction right) => new(left.Numerator * right.Numerator);
    }
}
