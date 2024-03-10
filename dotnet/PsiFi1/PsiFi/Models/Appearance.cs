using System;

namespace PsiFi.Models
{
    struct Appearance
    {
        /// <summary>
        /// Appearance of nothing.
        /// </summary>
        public static readonly Appearance Empty = new Appearance(' ', ConsoleColor.Gray);

        public char Character { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Appearance(char character, ConsoleColor foreColor, ConsoleColor backColor = ConsoleColor.Black)
        {
            Character = character;
            ForegroundColor = foreColor;
            BackgroundColor = backColor;
        }
    }
}