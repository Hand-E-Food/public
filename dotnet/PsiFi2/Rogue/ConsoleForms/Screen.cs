using System;
using System.Text;

namespace Rogue.ConsoleForms
{
    public class Screen
    {
        static Screen()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        private static Screen? lastScreen = null;

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

        public ControlCollection Controls { get; } = new ControlCollection();

        public void Invalidate()
        {
            if (lastScreen == this)
                lastScreen = null;
        }

        public ConsoleKeyInfo ReadKey()
        {
            Update();
            DateTime tooQuickTime;
            ConsoleKeyInfo key;
            do
            {
                tooQuickTime = DateTime.UtcNow.AddMilliseconds(10);
                key = Console.ReadKey(intercept: true);
            }
            while (DateTime.UtcNow < tooQuickTime);
            return key;
        }

        public void Update()
        {
            Console.CursorVisible = false;
            if (lastScreen != this)
            {
                lastScreen = this;
                RogueConsole.BackgroundColor = BackColor;
                Console.Clear();
                foreach (var control in Controls)
                    control.Invalidate();
            }

            foreach (var control in Controls)
                control.Update();
        }
    }
}
