using ConsoleForms;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays a log message with buttons.
    /// </summary>
    public class LogInteractionControl : ParentControl
    {
        /// <summary>
        /// Creates a new <see cref="LogInteractionControl"/>.
        /// </summary>
        /// <inheritdoc cref="ColoredText(string, ConsoleColor, ConsoleColor)"/>
        public LogInteractionControl(string text, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            textControl = new() {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 0,
            };
            textControl.SetText(text, foregroundColor, backgroundColor);
        }

        /// <summary>
        /// Creates a new <see cref="LogInteractionControl"/>.
        /// </summary>
        /// <inheritdoc cref="TextControl.SetText(ColoredText[])"/>
        public LogInteractionControl(params ColoredText[] text)
        {
            AnchorLeft = 0;
            AnchorTop = 0;
            AnchorRight = 0;

            textControl = new() {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 0,
            };
            textControl.SetText(text);
        }

        public override int? GetDesiredHeight()
        {
            PerformLayout();
            return textControl.GetDesiredHeight();
        }

        /// <summary>
        /// Returns a <see cref="TextControl"/> that will replace this control.
        /// </summary>
        /// <param name="answer">The extra text to add to the text control.</param>
        /// <returns>A <see cref="TextControl"/> ready to replace this control in the
        /// <see cref="LogControl"/>.</returns>
        public TextControl RemoveInteraction(string? answer = null)
        {
            Children.Clear();
            textControl.AnchorLeft = 0;
            if (!string.IsNullOrWhiteSpace(answer))
                textControl.Text = textControl.Text.Append(new(answer, ConsoleColor.Gray));
            return textControl;
        }

        private readonly TextControl textControl;

        /// <summary>
        /// Adds a button to this control.
        /// </summary>
        /// <param name="key">The key to press to activate the button.</param>
        /// <param name="action">The action performed when the button is pressed.</param>
        public void AddButton(char key, Action action)
        {
            Children.Add(new Button() {
                AnchorLeft = Children.OfType<Button>().Count(),
                AnchorTop = 0,
                Key = key,
                Action = action,
            });
            textControl.AnchorLeft = Children.Count;
        }
    }
}
