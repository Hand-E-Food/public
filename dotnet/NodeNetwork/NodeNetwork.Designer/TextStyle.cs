using System;
using System.Drawing;

namespace NodeNetwork.Designer
{
    /// <summary>
    /// A drawing instrument that encapsulates a <see cref="Font"/>, <see cref="Brush"/> and <see cref="StringFormat"/>.
    /// </summary>
    public class TextStyle : IDisposable
    {
        public Brush Brush { get; }

        public Font Font { get; }

        public StringFormat StringFormat { get; }

        public TextStyle(Font font, Brush brush, StringFormat stringFormat)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));
            if (brush == null)
                throw new ArgumentNullException(nameof(brush));

            Font = font;
            Brush = brush;
            StringFormat = stringFormat;
        }

        public void Dispose()
        {
            Font.Dispose();
            Brush.Dispose();
            StringFormat?.Dispose();
        }

        public static implicit operator Brush(TextStyle textFormat) => textFormat.Brush;

        public static implicit operator Font(TextStyle textFormat) => textFormat.Font;

        public static implicit operator StringFormat(TextStyle textFormat) => textFormat.StringFormat;
    }
}
