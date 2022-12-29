using ConsoleForms;
using System.ComponentModel;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays a <see cref="Range"/> object.
    /// </summary>
    internal class RangeControl : Control
    {
        public override int? GetDesiredHeight() => 1;

        /// <summary>
        /// The color to draw with.
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => foregroundColor;
            set
            {
                if (foregroundColor == value) return;
                foregroundColor = value;
                InvalidateDrawing();
            }
        }
        private ConsoleColor foregroundColor = ConsoleColor.White;

        public IEnumerable<RangeFormat> Formats
        {
            get => formats;
            set
            {
                formats = value;
                InvalidateDrawing();
            }
        }
        private IEnumerable<RangeFormat> formats = new[]
        {
            RangeFormat.Fraction,
            RangeFormat.Percentage,
            RangeFormat.Value,
            RangeFormat.Overflow("!!!")
        };

        /// <summary>
        /// The range to display.
        /// </summary>
        public Range? Range
        {
            get => range;
            set
            {
                if (range == value) return;
                if (range != null) range.PropertyChanged -= Range_PropertyChanged;
                range = value;
                if (range != null) range.PropertyChanged += Range_PropertyChanged;
                InvalidateDrawing();
            }
        }
        private Range? range = null;

        private void Range_PropertyChanged(object? sender, PropertyChangedEventArgs e) =>
            InvalidateDrawing();

        protected override void Draw(Graphics graphics)
        {
            if (Range == null) return;
            var width = Bounds.Width;
            var format = Formats.FirstOrDefault(format => format.MinimumWidth(Range) <= width);
            if (format == null) return;
            var text = format.Format(Range);
            graphics.SetCursorPosition(Bounds.TopLeft);
            graphics.Write(text, ForegroundColor, BackgroundColor);
        }
    }
}
