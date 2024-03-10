using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// A pie shaped portion of a circle.
    /// </summary>
    public class Pie : Circle
    {

        /// <summary>
        /// The rectangular bounds of this shape.
        /// </summary>
        public override RectangleF Bounds
        {
            get
            {
                if (Math.Abs(SweepAngle) >= 360)
                    return CircleBounds;

                var start = NormalizeDeg(StartAngle);
                var end = NormalizeDeg(StartAngle + SweepAngle);
                if (SweepAngle < 0)
                {
                    var swap = start;
                    start = end;
                    end = swap;
                }

                var points = new List<PointF>();
                points.Add(Centre);
                points.Add(Vector.Polar(Centre, start, Radius));
                points.Add(Vector.Polar(Centre, end, Radius));
                if (start <= end) 
                {
                    if (start <  90f && end >  90f) points.Add(new PointF(Centre.X + Radius, Centre.Y));
                    if (start < 180f && end > 180f) points.Add(new PointF(Centre.X, Centre.Y - Radius));
                    if (start < 270f && end > 270f) points.Add(new PointF(Centre.X - Radius, Centre.Y));
                }
                else
                {
                    if (end >   0f) points.Add(new PointF(Centre.X, Centre.Y + Radius));
                    if (end >  90f) points.Add(new PointF(Centre.X + Radius, Centre.Y));
                    if (end > 180f) points.Add(new PointF(Centre.X, Centre.Y - Radius));
                    if (end > 270f) points.Add(new PointF(Centre.X - Radius, Centre.Y));
                }

                var location = new PointF(points.Min(p => p.X), points.Min(p => p.Y));
                var size = new SizeF(points.Max(p => p.X) - location.X, points.Max(p => p.Y) - location.Y);
                return new RectangleF(location, size);
            }
        }

        /// <summary>
        /// The angle in degrees at which to start drawing the arc for <see cref="Graphics"/> operations.
        /// </summary>
        protected float GraphicsStartAngle { get { return 90f - StartAngle; } }

        /// <summary>
        /// The angle in degrees over which to draw the arc counter-clockwise from the StartAngle for
        /// <see cref="Graphics"/> operations.
        /// </summary>
        protected float GraphicsSweepAngle { get { return -SweepAngle; } }

        /// <summary>
        /// The angle in degrees at which to start drawing the arc.
        /// </summary>
        public float StartAngle { get; set; }

        /// <summary>
        /// The angle in degrees over which to draw the arc counter-clockwise from the StartAngle.
        /// </summary>
        public float SweepAngle { get; set; }

        /// <summary>
        /// Draws this shape.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public override void Draw(Graphics g)
        {
            ThrowIfDisposed();
            if (Pen == null) return;
            g.DrawPie(Pen, CircleBounds, GraphicsStartAngle, GraphicsSweepAngle);
        }

        /// <summary>
        /// Fills this shape.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public override void Fill(Graphics g)
        {
            ThrowIfDisposed();
            if (Brush == null) return;
            g.FillPie(Brush, CircleBounds.X, CircleBounds.Y, CircleBounds.Width, CircleBounds.Height, GraphicsStartAngle, GraphicsSweepAngle);
        }
    }
}
