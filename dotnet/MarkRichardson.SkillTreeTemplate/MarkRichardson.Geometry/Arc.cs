using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// A circular arc.
    /// </summary>
    public class Arc : Pie
    {

        /// <summary>
        /// Draws this shape.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public override void Draw(Graphics g)
        {
            ThrowIfDisposed();
            if (Pen == null) return;
            g.DrawArc(Pen, CircleBounds, GraphicsStartAngle, GraphicsSweepAngle);
        }

        /// <summary>
        /// Fills this shape.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public override void Fill(Graphics g)
        {
            ThrowIfDisposed();
        }
    }
}
