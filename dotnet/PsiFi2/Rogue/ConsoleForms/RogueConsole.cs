using System;

namespace Rogue.ConsoleForms
{
    public static class RogueConsole
    {

        public static Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (backgroundColor == value) return;
                backgroundColor = value;
                Console.Write(value.ToVT100BackgroundString());
            }
        }
        private static Color backgroundColor = Color.Black;

        public static Color ForegroundColor
        {
            get => foregroundColor;
            set
            {
                if (foregroundColor == value) return;
                foregroundColor = value;
                Console.Write(value.ToVT100ForegroundString());
            }
        }
        private static Color foregroundColor = Color.Black;

        public static void Write(Color backgroundColor, Color foregroundColor, char text)
        {
            Console.Write(backgroundColor.ToVT100BackgroundString() + foregroundColor.ToVT100ForegroundString() + text);
            RogueConsole.backgroundColor = backgroundColor;
            RogueConsole.foregroundColor = foregroundColor;
        }

        public static void Write(Color backgroundColor, Color foregroundColor, string text)
        {
            Console.Write(backgroundColor.ToVT100BackgroundString() + foregroundColor.ToVT100ForegroundString() + text);
            RogueConsole.backgroundColor = backgroundColor;
            RogueConsole.foregroundColor = foregroundColor;
        }
    }
}
