using System;
using System.Text.RegularExpressions;

namespace PsiFi.Geometry
{
    /// <summary>
    /// A combination of dice that can be rolled.
    /// </summary>
    class Dice
    {
        private const string DicePattern = @"[0-9]+(d[1-9][0-9]*)?(\*[0-9]+)?";
        private static readonly Regex ValidFormat = new Regex(@$"^{DicePattern}([+\-]{DicePattern})*$", RegexOptions.Compiled);

        private string dice;

        public Dice(string dice)
        {
            if (!ValidFormat.IsMatch(dice)) throw new FormatException($"\"{dice}\" is not a valid dice format.");
            this.dice = dice;
        }

        public int Roll(Random random)
        {
            var total = 0;
            var reader = new DiceReader(dice);
            var operation = '+';

            while (reader.HasMore)
            {
                var quantity = reader.ReadInt32();
                var size = 1;
                var multiplier = operation == '+' ? 1 : -1;
                operation = reader.ReadChar();
                if (operation == 'd')
                {
                    size = reader.ReadInt32();
                    operation = reader.ReadChar();
                }
                if (operation == '*')
                {
                    multiplier *= reader.ReadInt32();
                    operation = reader.ReadChar();
                }

                if (size > 1)
                {
                    while (quantity-- > 0)
                        total += (1 + random.Next(size)) * multiplier;
                }
                else
                {
                    total += quantity * multiplier;
                }

            }
            return total;
        }

        private class DiceReader
        {
            private readonly string dice;
            
            private int i = 0;

            public DiceReader(string dice)
            {
                this.dice = dice;
            }

            /// <summary>
            /// True if this reader has more to read.
            /// False if the reader has read to the end of the string.
            /// </summary>
            public bool HasMore => i < dice.Length;

            /// <summary>
            /// Returns the next consecutive digits as an integer.
            /// </summary>
            /// <returns>An integer.</returns>
            public int ReadInt32()
            {
                var j = i + 1;
                while (j < dice.Length && char.IsDigit(dice[j]))
                    j++;
                var n = int.Parse(dice.Substring(i, j - i));
                i = j;
                return n;
            }

            /// <summary>
            /// Returns the next character. If the end of the string is reached, returns a null terminator.
            /// </summary>
            /// <returns>
            /// The next character.
            /// If the end of the string is reached, returns a null terminator.
            /// </returns>
            public char ReadChar() => HasMore ? dice[i++] : '\0';
        }
    }
}
