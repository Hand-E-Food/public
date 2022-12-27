namespace ConsoleForms
{
    /// <summary>
    /// A button that can be pressed with a key.
    /// </summary>
    public class Button : Control
    {
        /// <summary>
        /// Creates a new <see cref="Button"/>.
        /// </summary>
        /// <param name="key">The key to press to activate this button.</param>
        /// <param name="action">The action performed when this button is presseed.</param>
        public Button(char key, Action action)
        {
            BackgroundColor = ConsoleColor.DarkGreen;
            Key = key;
            Action = action;
        }

        /// <summary>
        /// This button's text color when enabled.
        /// </summary>
        public ConsoleColor EnabledColor { get; set; } = ConsoleColor.Green;

        /// <summary>
        /// This button's text color when disabled.
        /// </summary>
        public ConsoleColor DisabledColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// True if this button is enabled. False if this button is disabled.
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                Invalidate();
            }
        }
        private bool isEnabled = true;

        /// <summary>
        /// The key to press to activate this button.
        /// </summary>
        public char Key { get; set; }

        /// <summary>
        /// The action performed when this button is pressed.
        /// </summary>
        public Action Action { get; set; }

        public override void Draw(Graphics graphics)
        {
            base.Draw(graphics);
            var foregroundColor = IsEnabled ? EnabledColor : DisabledColor;
            graphics.SetCursorPosition(Left, Top);
            graphics.Write(Key.ToString(), foregroundColor, BackgroundColor);
        }
    }
}
