using System.Diagnostics;

namespace ConsoleForms
{
    /// <summary>
    /// Text with colour.
    /// </summary>
    public class ColoredText
    {
        public override string ToString() => $"\"{Text}\" {ForegroundColor}/{BackgroundColor}";

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

        /// <inheritdoc cref="String.Remove(int)"/>
        public ColoredText Remove(int startIndex) => Clone(Text.Remove(startIndex));

        /// <inheritdoc cref="String.Remove(int, int)"/>
        public ColoredText Remove(int startIndex, int count) => Clone(Text.Remove(startIndex, count));

        /// <inheritdoc cref="String.Substring(int)"/>
        public ColoredText Substring(int startIndex) => Clone(Text.Substring(startIndex));

        /// <inheritdoc cref="String.Substring(int, int)"/>
        public ColoredText Substring(int startIndex, int length) => Clone(Text.Substring(startIndex, length));

        private ColoredText Clone(string text) => new(text, ForegroundColor, BackgroundColor);

        /// <summary>
        /// This text's background color.
        /// </summary>
        public readonly ConsoleColor BackgroundColor;

        /// <summary>
        /// This text's foreground color.
        /// </summary>
        public readonly ConsoleColor ForegroundColor;

        /// <summary>
        /// The text's length.
        /// </summary>
        public int Length => Text.Length;

        /// <summary>
        /// The text to write.
        /// </summary>
        public readonly string Text;
    }
}
