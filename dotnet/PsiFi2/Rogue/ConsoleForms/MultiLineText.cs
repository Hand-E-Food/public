using System;
using System.Linq;

namespace Rogue.ConsoleForms
{
    public class MultiLineText : Control
    {
        public Color ForeColor
        {
            get => foreColor;
            set
            {
                if (foreColor == value) return;
                foreColor = value;
                Invalidate();
            }
        }
        private Color foreColor = Color.Gray;

        public HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value) return;
                horizontalAlignment = value;
                Invalidate();
            }
        }
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left;

        public string Text
        {
            get => text;
            set
            {
                if (value == null) value = string.Empty;
                if (text == value) return;
                text = value;
                Invalidate();
            }
        }
        private string text = string.Empty;

        public VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value) return;
                verticalAlignment = value;
                Invalidate();
            }
        }
        private VerticalAlignment verticalAlignment = VerticalAlignment.Top;

        protected override void Paint()
        {
            var lines = text
                .Split(Environment.NewLine)
                .SelectMany(text => text.Wrap(Bounds.Width))
                .ToList();

            if (lines.Capacity < Bounds.Height)
                lines.Capacity = Bounds.Height;
            
            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    if (lines.Count < Bounds.Height)
                        lines.Add(string.Empty);
                    break;

                case VerticalAlignment.Bottom:
                    while (lines.Count < Bounds.Height)
                        lines.Insert(0, string.Empty);
                    if (lines.Count > Bounds.Height)
                        lines.RemoveRange(0, lines.Count - Bounds.Height);

                    break;
            }

            RogueConsole.ForegroundColor = ForeColor;
            for (int i = 0, y = Bounds.Y; i < Bounds.Height; i++, y++)
            {
                Console.SetCursorPosition(Bounds.X, y);
                Console.Write(lines[i].Align(HorizontalAlignment, Bounds.Width));
            }
        }
    }
}
