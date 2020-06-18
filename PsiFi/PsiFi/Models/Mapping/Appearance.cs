using System;

namespace PsiFi.Models.Mapping
{
    class Appearance
    {
        public static readonly Appearance None = new Appearance(' ', ConsoleColor.Gray);

        public char Character { get; }
        public ConsoleColor ForeColor { get; }
        public ConsoleColor BackColor { get; }

        public Appearance(char character, ConsoleColor foreColor, ConsoleColor backColor = ConsoleColor.Black)
        {
            Character = character;
            ForeColor = foreColor;
            BackColor = backColor;
        }
    }
}
