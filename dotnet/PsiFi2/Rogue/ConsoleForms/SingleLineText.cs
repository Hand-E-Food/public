using System;

namespace Rogue.ConsoleForms
{
    public class SingleLineText : Control
    {
        public override Rectangle Bounds
        {
            get => base.Bounds;
            set
            {
                if (value.Height != 1)
                    throw new ArgumentOutOfRangeException(nameof(Bounds), $"{nameof(SingleLineText)}.{nameof(Bounds)} must have {nameof(Rectangle.Height)} of 1");

                base.Bounds = value;
            }
        }

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

        public string Text {
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

        protected override void Paint()
        {
            Console.SetCursorPosition(Bounds.X, Bounds.Y);
            RogueConsole.Write(BackColor, ForeColor, Text.Align(HorizontalAlignment, Bounds.Width));
        }
    }
}
