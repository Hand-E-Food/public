using Rogue.Mapping;
using System;

namespace Rogue.ConsoleForms
{
    /// <summary>
    /// A control showing part or all of a map.
    /// </summary>
    /// <typeparam name="TCell">The type of cells in the map.</typeparam>
    public abstract class MapView<TCell> : Control where TCell : ICell
    {
        /// <summary>
        /// The map to display.
        /// </summary>
        public IMap<TCell> Map
        { 
            get => map;
            set
            {
                if (map == value) return;
                map = value;
                Invalidate();
            } 
        }
        private IMap<TCell> map = null!;

        /// <summary>
        /// The map cell shown in the top-left corner of this control.
        /// </summary>
        public Point Offset
        {
            get => offset;
            set
            {
                if (offset == value) return;
                offset = value;
                Invalidate();
            }
        }
        private Point offset = Point.Empty;

        /// <inheritdoc/>
        protected override void Paint()
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            for (int j = Bounds.Top, y = Offset.Y; j < Bounds.Bottom; j++, y++)
            {
                Console.SetCursorPosition(Bounds.Left, j);
                for (int i = Bounds.Left, x = Offset.X; i < Bounds.Right; i++, x++)
                {
                    if (x >= 0 && x < map.Size.Width && y >= 0 && y < map.Size.Height)
                    {
                        PaintCell(map[x, y]);
                    }
                    else
                    {
                        RogueConsole.Write(BackColor, Color.Gray, ' ');
                    }
                }
            }
        }

        /// <summary>
        /// Paints the cell's contents to the current cursor location.
        /// </summary>
        /// <param name="cell">The cell to paint.</param>
        /// <remarks>The cursor is assumed to be in the correct position before entering this method.</remarks>
        protected abstract void PaintCell(TCell cell);
    }
}
