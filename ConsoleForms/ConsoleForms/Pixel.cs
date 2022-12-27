namespace ConsoleForms
{
    /// <summary>
    /// A textual pixel.
    /// </summary>
    public struct Pixel
    {

        /// <summary>
        /// Creates a new <see cref="Pixel"/>.
        /// </summary>
        /// <param name="character">The character to display in this pixel.</param>
        /// <param name="foregroundColor">This pixel's foreground color.</param>
        /// <param name="backgroundColor">This pixel's background color.</param>
        public Pixel(char character, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Character = character;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// The character to display in this pixel.
        /// </summary>
        public char Character;

        /// <summary>
        /// This pixel's foreground color.
        /// </summary>
        public ConsoleColor ForegroundColor;

        /// <summary>
        /// This pixel's background color.
        /// </summary>
        public ConsoleColor BackgroundColor;
    }
}
