using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// A circle.
    /// </summary>
    public class Circle : Shape
    {

        /// <summary>
        /// The rectangle encompasing the circle.
        /// </summary>
        public override RectangleF Bounds 
        {
            get { return CircleBounds; }
        }

        /// <summary>
        /// The centre of the circle.
        /// </summary>
        public PointF Centre { get; set; }


        protected RectangleF CircleBounds
        {
            get { return new RectangleF(Centre.X - Radius, Centre.Y - Radius, Radius * 2, Radius * 2); }
        }

        /// <summary>
        /// The circle's radius.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Draws this shape.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public override void Draw(Graphics g)
        {
            ThrowIfDisposed();
            if (Pen == null) return;
            g.DrawEllipse(Pen, CircleBounds);
        }

        /// <summary>
        /// Fills this shape.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public override void Fill(Graphics g)
        {
            ThrowIfDisposed();
            if (Brush == null) return;
            g.FillEllipse(Brush, CircleBounds);
        }

        /// <summary>
        /// Returns Centre.
        /// </summary>
        /// <returns>Centre.</returns>
        public static implicit operator PointF(Circle c)
        {
            return c.Centre;
        }

        /// <summary>
        /// Returns Bounds.
        /// </summary>
        /// <returns>Bounds.</returns>
        public static implicit operator RectangleF(Circle c)
        {
            return c.Bounds;
        }
    }
}
