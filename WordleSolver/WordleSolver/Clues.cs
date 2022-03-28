using System;
using System.Linq;
using static WordleSolver.Constants;

namespace WordleSolver
{
    public sealed class Clues : IEquatable<Clues>
    {
        /// <summary>
        /// The index represented by clues that are all <see cref="Clue.Correct">.
        /// </summary>
        public static readonly int Correct = Clues.FromString(new('2', WordLength)).GetHashCode();

        /// <summary>
        /// The size to give to arrays where each index represents clues.
        /// </summary>
        public static readonly int ArrayLength = Correct + 1;

        private readonly int[] clues = new int[WordLength];

        public static Clues FromHashCode(int hashCode)
        {
            Clues result = new();
            for (int i = WordLength - 1; i >= 0; i--)
            {
                result.clues[i] = hashCode % 3;
                hashCode /= 3;
            }
            return result;
        }

        public static Clues FromString(string str)
        {
            Clues result = new();
            for (int i = 0; i < WordLength; i++)
                result.clues[i] = str[i] - '0';
            return result;
        }

        public Clue this[int i]
        {
            get => (Clue)clues[i];
            set => clues[i] = (int)value;
        }

        public bool Equals(Clues other)
        {
            for (int i = 0; i < WordLength; i++)
                if (clues[i] != other.clues[i]) return false;
            return true;
        }

        public override bool Equals(object obj) => obj is Clues other && Equals(other);

        public override int GetHashCode() => clues.Aggregate((a, b) => a * 3 + b);

        public override string ToString() => string.Join(string.Empty, clues);

        public static implicit operator Clues(string clues) => Clues.FromString(clues);
        public static implicit operator string(Clues clues) => clues.ToString();
        public static implicit operator Clues(int clues) => Clues.FromHashCode(clues);
        public static implicit operator int(Clues clues) => clues.GetHashCode();
    }
}
