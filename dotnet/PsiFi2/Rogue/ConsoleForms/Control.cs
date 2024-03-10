using System;

namespace Rogue.ConsoleForms
{
    public abstract class Control
    {
        private bool isInvalid = true;

        public Color BackColor
        { 
            get => backColor;
            set
            {
                if (backColor == value) return;
                backColor = value;
                Invalidate();
            }
        }
        private Color backColor = Color.Black;

        public virtual Rectangle Bounds 
        {
            get => bounds.HasValue ? bounds.Value : throw new ArgumentNullException(nameof(Bounds));
            set
            {
                if (value.X < 0 || value.Y < 0 || value.Width < 0 || value.Height < 0)
                    throw new ArgumentOutOfRangeException(nameof(Bounds), $"{nameof(Bounds)} must not have negative values");

                if (bounds == value) return;
                if (bounds.HasValue) Clear();
                bounds = value;
                Invalidate();
            }
        }
        private Rectangle? bounds = null;

        public bool Visible {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                isInvalid = true;
            }
        }
        private bool visible = true;

        public void Clear()
        {
            RogueConsole.BackgroundColor = BackColor;
            Console.CursorTop = Bounds.Top;
            var spaces = new string(' ', Bounds.Width);
            for (int y = Bounds.Top; y < Bounds.Bottom; y++)
            {
                Console.CursorLeft = Bounds.Left;
                Console.Write(spaces);
            }
        }

        public void Invalidate() => isInvalid = true;

        protected virtual void Paint() => Clear();

        internal void Update()
        {
            if (isInvalid)
            {
                if (Visible)
                {
                    RogueConsole.BackgroundColor = BackColor;
                    Paint();
                }
                else
                {
                    Clear();
                }
                isInvalid = false;
            }
        }
    }
}
