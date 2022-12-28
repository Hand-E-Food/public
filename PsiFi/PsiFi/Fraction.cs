using System.Diagnostics.CodeAnalysis;

namespace PsiFi
{
    public struct Fraction : IComparable<Fraction>, IComparable<int>, IEquatable<Fraction>
    {
        public const int Denominator = 1 * 2 * 3 * 2 * 5 * 1 * 7 * 2 * 3 * 1; //* 11 * 1 * 13 * 1 * 1 * 2 * 17 * 1 * 19 * 1;

        public const int MaxDenominator = 10;

        public const int MaxValue = int.MaxValue / Denominator;

        public const int MinValue = int.MinValue / Denominator;

        public int Numerator;

        public double AsDouble => (double)Numerator / Denominator;

        private Fraction(int numerator)
        {
            Numerator = numerator;
        }

        public Fraction(int numerator, int denominator)
        {
            if (denominator < 1 || denominator > MaxDenominator)
                throw new ArgumentException($"{nameof(denominator)} must be betweeen 1 and {nameof(MaxDenominator)}.");
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
