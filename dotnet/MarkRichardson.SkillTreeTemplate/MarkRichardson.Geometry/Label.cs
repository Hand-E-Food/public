using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// Represents a simple text label.
    /// </summary>
    public class Label : Shape
    {

        public override RectangleF Bounds
        { //TODO: Set Label bounds.
            get { return RectangleF.Empty; }
        }

        /// <summary>
        /// The centre of the label's text.
        /// </summary>
        public PointF Centre { get; set; }

        /// <summary>
        /// The font used to draw this label.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// The angle in degrees to rotate this label.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The string format for the label.
        /// </summary>
        public StringFormat StringFormat { get; set; }

        /// <summary>
        /// The label's text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
        {
            StringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        }

        /// <summary>
        /// Draws this shape.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public override void Draw(Graphics g)
        {
            ThrowIfDisposed();
        }

        /// <summary>
        /// Fills this shape.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public override void Fill(Graphics g)
        {
            ThrowIfDisposed();
            if (Brush == null) return;
            var transform = g.Transform;
            try
            {
                g.TranslateTransform(Centre.X, Centre.Y);
                g.RotateTransform(-Rotation);
                g.TranslateTransform(-Centre.X, -Centre.Y);
                g.DrawString(Text, Font, Brush, Centre, StringFormat);
            }
            finally
            {
                g.Transform = transform;
            }
        }

        /// <summary>
        /// Returns Text.
        /// </summary>
        /// <returns>Text.</returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// Returns Centre.
        /// </summary>
        /// <returns>Centre.</returns>
        public static implicit operator PointF(Label l)
        {
            return l.Centre;
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        /// <param name="disposing">True to dispose of managed resources; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!IsDisposed)
                {
                    if (disposing)
                    {
                        if (Font != null) Font.Dispose();
                        if (StringFormat != null) StringFormat.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
