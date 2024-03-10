using System;
using System.Collections.Generic;

namespace Rogue.ConsoleForms
{
    class Cursor : Control
    {
        public override Rectangle Bounds 
        { 
            get => base.Bounds;
            set
            {
                if (value.Width != 1 || value.Height != 1)
                    throw new ArgumentOutOfRangeException(nameof(Bounds), $"{nameof(Bounds)} must have a {nameof(Rectangle.Width)} and {nameof(Rectangle.Height)} of 1");

                base.Bounds = value;
            }
        }

        public char Character 
        {
            get => character;
            set
            {
                if (character == value) return;
                character = value;
                Invalidate();
            }
        }
        private char character = '>';

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

        public List<Point> Positions { get; } = new List<Point>();

        public int SelectedPosition
        {
            get => selectedPosition;
            set
            {
                if (selectedPosition == value) return;
                if (value < 0 || value >= Positions.Count)
                    throw new ArgumentOutOfRangeException(nameof(SelectedPosition), $"{nameof(SelectedPosition)} must be a valid index of {nameof(Positions)}");

                selectedPosition = value;
                base.Bounds = new Rectangle(Positions[selectedPosition], new Size(1, 1));
            }
        }
        private int selectedPosition = 0;

        protected override void Paint()
        {
            Console.SetCursorPosition(Bounds.X, Bounds.Y);
            RogueConsole.Write(BackColor, ForeColor, character);
        }
    }
}
