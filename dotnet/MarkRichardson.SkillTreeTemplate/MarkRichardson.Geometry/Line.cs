using System;
using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// A line between two circles.
    /// </summary>
    public class Line : Shape
    {

        /// <summary>
        /// The rectangular bounds of this shape.
        /// </summary>
        public override RectangleF Bounds
        {
            get
            {
                return new RectangleF(
                    Math.Min(Start.Centre.X, End.Centre.X),
                    Math.Min(Start.Centre.Y, End.Centre.Y),
                    Math.Abs(Start.Centre.X - End.Centre.X),
                    Math.Abs(Start.Centre.X - End.Centre.X));
            }
        }

        /// <summary>
        /// The line's end point.
        /// </summary>
        public Circle End { get; set; }

        /// <summary>
        /// The line's starting point.
        /// </summary>
        public Circle Start { get; set; }

        /// <summary>
        /// Draws this shape.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public override void Draw(Graphics g)
        {
            ThrowIfDisposed();
            if (Pen == null) return;
            var deg = Vector.AngleTo(Start, End);
            g.DrawLine(Pen, Vector.Polar(Start, deg, Start.Radius), Vector.Polar(End, deg, -End.Radius));
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
