namespace ConsoleForms
{
    /// <summary>
    /// Characters to represent non-printing keys.
    /// </summary>
    public static class Key
    {
        //public const char AltModifier = '⎇';
        public const char Backspace = '␈';
        //public const char ControlModifier = '^';
        public const char Delete = '␡';
        public const char DownArrow = '↓';
        public const char End = '␃';
        public const char Enter = '⏎';
        public const char Escape = '␛';
        public const char Home = '␂';
        public const char Insert = '⎀';
        public const char LeftArrow = '←';
        public const char None = '\x00';
        public const char PageDown = '⎘';
        public const char PageUp = '⎗';
        public const char RightArrow = '→';
        //public const char ShiftModifier = '⇧';
        public const char Tab = '⭾';
        public const char UpArrow = '↑';
        public static class Shift
        {
            public const char Backspace = '\x06'; // acknowledge
            public const char Delete = '\x15'; // negative acknowledge
            public const char DownArrow = '⇓';
            public const char End = '\x1F'; // unit separator
            public const char Enter = '\x17'; // end of transmission block
            public const char Escape = '\x18'; // cancel
            public const char Home = '\x1C'; // file separator
            public const char Insert = '\x07'; // bell
            public const char LeftArrow = '⇐';
            public const char PageDown = '\x1E'; // record separator
            public const char PageUp = '\x1D'; // group separator
            public const char RightArrow = '⇒';
            public const char Tab = '\x11'; // vertical tab
            public const char UpArrow = '⇑';
        }

        /// <summary>
        /// Reads the next key press.
        /// </summary>
        /// <returns>The pressed key.</returns>
        public static char Read() => Parse(Console.ReadKey(intercept: true));

        /// <summary>
        /// Parses the key info into a single character.
        /// </summary>
        /// <param name="keyInfo">The <see cref="ConsoleKeyInfo"/> to parse.</param>
        /// <returns>A single character that represents that key.</returns>
        public static char Parse(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Modifiers == 0)
            {
                return keyInfo.Key switch
                {
                    ConsoleKey.Backspace => Backspace,
                    ConsoleKey.Delete => Delete,
                    ConsoleKey.DownArrow => DownArrow,
                    ConsoleKey.End => End,
                    ConsoleKey.Enter => Enter,
                    ConsoleKey.Escape => Escape,
                    ConsoleKey.Home => Home,
                    ConsoleKey.Insert => Insert,
                    ConsoleKey.LeftArrow => LeftArrow,
                    ConsoleKey.PageDown => PageDown,
                    ConsoleKey.PageUp => PageUp,
                    ConsoleKey.RightArrow => RightArrow,
                    ConsoleKey.Tab => Tab,
                    ConsoleKey.UpArrow => UpArrow,
                    _ => keyInfo.KeyChar
                };
            }
            else if (keyInfo.Modifiers == ConsoleModifiers.Shift)
            {
                return keyInfo.Key switch
                {
                    ConsoleKey.Backspace => Shift.Backspace,
                    ConsoleKey.Delete => Shift.Delete,
                    ConsoleKey.DownArrow => Shift.DownArrow,
                    ConsoleKey.End => Shift.End,
                    ConsoleKey.Enter => Shift.Enter,
                    ConsoleKey.Escape => Shift.Escape,
                    ConsoleKey.Home => Shift.Home,
                    ConsoleKey.Insert => Shift.Insert,
                    ConsoleKey.LeftArrow => Shift.LeftArrow,
                    ConsoleKey.PageDown => Shift.PageDown,
                    ConsoleKey.PageUp => Shift.PageUp,
                    ConsoleKey.RightArrow => Shift.RightArrow,
                    ConsoleKey.Tab => Shift.Tab,
                    ConsoleKey.UpArrow => Shift.UpArrow,
                    _ => keyInfo.KeyChar
                };
            }
            else
            {
                return None;
            }
        }
    }
}
