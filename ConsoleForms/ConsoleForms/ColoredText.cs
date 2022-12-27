namespace ConsoleForms
{
    /// <summary>
    /// Text with colour.
    /// </summary>
    public class ColoredText
    {
        /// <summary>
        /// Creates a new <see cref="ColoredText"/>.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="foregroundColor">This text's foreground color.</param>
        /// <param name="backgroundColor">This text's background color.</param>
        public ColoredText(string text, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// This text's background color.
        /// </summary>
        public readonly ConsoleColor BackgroundColor;

        /// <summary>
        /// This text's foreground color.
        /// </summary>
        public readonly ConsoleColor ForegroundColor;

        /// <summary>
        /// The text to write.
        /// </summary>
        public readonly string Text;
    }
}
