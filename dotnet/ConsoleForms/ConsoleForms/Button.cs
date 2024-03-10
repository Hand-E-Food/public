namespace ConsoleForms
{
    /// <summary>
    /// A button control that handles a key press.
    /// </summary>
    public class Button : Control
    {
        /// <summary>
        /// This button's text color when enabled.
        /// </summary>
        public ConsoleColor EnabledColor
        {
            get => enabledColor;
            set
            {
                if (enabledColor == value) return;
                enabledColor = value;
                InvalidateDrawing();
            }
        }
        private ConsoleColor enabledColor = ConsoleColor.Green;

        /// <summary>
        /// This button's text color when disabled.
        /// </summary>
        public ConsoleColor DisabledColor
        {
            get => disabledColor;
            set
            {
                if (disabledColor == value) return;
                disabledColor = value;
                InvalidateDrawing();
            }
        }
        private ConsoleColor disabledColor = ConsoleColor.Black;

        /// <summary>
        /// True if this button is enabled. False if this button is disabled.
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value) return;
                isEnabled = value;
                InvalidateDrawing();
            }
        }
        private bool isEnabled = true;

        /// <summary>
        /// The key to press to activate this button.
        /// </summary>
        public char? Key
        {
            get => key;
            set
            {
                if (key == value) return;
                key = value;
                InvalidateDrawing();
            }
        }
        private char? key = null;

        /// <summary>
        /// The action performed when this button is pressed.
        /// </summary>
        public Action Action { get; set; } = NoAction;

        /// <summary>
        /// Does nothing. The default value of <see cref="Action"/>.
        /// </summary>
        public static void NoAction() { }

        protected override void Draw(Graphics graphics)
        {
            var text = Key?.ToString() ?? string.Empty;
            var foregroundColor = IsEnabled ? EnabledColor : DisabledColor;
            graphics.SetCursorPosition(Bounds.TopLeft);
            graphics.Write(text, foregroundColor, BackgroundColor);
        }

        public override bool HandleKey(char key)
        {
            var handled = IsEnabled && key == Key;
            if (handled) Action();
            return handled;
        }
    }
}
