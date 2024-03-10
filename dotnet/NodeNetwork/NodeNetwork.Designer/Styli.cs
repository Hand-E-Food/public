using System;
using System.Drawing;

namespace NodeNetwork.Designer
{
    /// <summary>
    /// A drawing instrument that can act as a <see cref="Pen"/> or a <see cref="Brush"/>.
    /// </summary>
    public class Styli : IDisposable
    {

        public Brush Brush { get; }

        public Pen Pen { get; }

        public Styli(Brush brush, Pen pen)
        {
            Brush = brush;
            Pen = pen;
        }

        public Styli(Color color)
        {
            Brush = new SolidBrush(color);
            Pen = new Pen(color);
        }

        public Styli(Color color, int brushAlpha, int penAlpha)
        {
            Brush = new SolidBrush(Color.FromArgb(brushAlpha, color));
            Pen = new Pen(Color.FromArgb(penAlpha, color));
        }

        public void Dispose()
        {
            Brush?.Dispose();
            Pen?.Dispose();
        }


        public static implicit operator Brush(Styli styli) => styli.Brush;

        public static implicit operator Pen(Styli styli) => styli.Pen;
    }
}
