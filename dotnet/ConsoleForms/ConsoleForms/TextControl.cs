namespace ConsoleForms
{
    public class TextControl : Control
    {
        public override int? GetDesiredHeight() => Math.Min(MaximumHeight.GetValueOrDefault(int.MaxValue), GetTextLines().Count());

        /// <summary>
        /// This control's maximum desired height.
        /// </summary>
        public int? MaximumHeight
        {
            get => maximumHeight;
            set
            {
                if (maximumHeight == value) return;
                maximumHeight = value;
                InvalidateLayout();
            }
        }
        private int? maximumHeight = null;

        protected override void Draw(Graphics graphics)
        {
            graphics.CursorY = Bounds.Top;
            foreach (var line in GetTextLines())
            {
                graphics.CursorX = Bounds.Left;
                foreach (var text in line) graphics.Write(text);
                graphics.CursorY++;
            }
        }

        private IEnumerable<IEnumerable<ColoredText>> GetTextLines()
        {
            List<ColoredText> line = new();
            int width = Bounds.Width;
            int length = 0;
            var enumerator = Text.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var text = enumerator.Current;
                var remaining = width - length;
                while (text.Length > remaining)
                {
                    int index = text.Text.LastIndexOf(' ', remaining);
                    if (index >= 0)
                    {
                        line.Add(text.Remove(index));
                        text = text.Substring(index + 1);
                    }
                    else if (line.Count == 0)
                    {
                        line.Add(text.Remove(remaining));
                        text = text.Substring(remaining);
                    }
                    yield return line.ToArray();
                    line.Clear();
                    length = 0;
                    remaining = width;
                }
                if (text.Text.Length > 0)
                {
                    line.Add(text);
                    length += text.Length;
                }
            }
            if (line.Count > 0) yield return line.ToArray();
        }

        /// <summary>
        /// The text displayed by this control.
        /// </summary>
        public IEnumerable<ColoredText> Text
        {
            get => text;
            set => SetText(value?.ToArray() ?? Array.Empty<ColoredText>());
        }
        /// <summary>
        /// Sets the text displayed in this control.
        /// </summary>
        /// <inheritdoc cref="ColoredText.ColoredText(string, ConsoleColor, ConsoleColor)"/>
        public void SetText(string text, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black) =>
            SetText(new[] { new ColoredText(text, foregroundColor, backgroundColor) });
        /// <summary>
        /// Sets the text displayed in this control.
        /// </summary>
        /// <param name="text">The text displayed by this control.</param>
        public void SetText(params ColoredText[] text)
        {
            this.text = text;
            InvalidateLayout();
        }

        private ColoredText[] text = Array.Empty<ColoredText>();
    }
}
