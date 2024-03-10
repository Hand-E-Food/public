using System;
using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// The base class for shapes.
    /// </summary>
    public abstract class Shape : IDisposable
    {

        /// <summary>
        /// The rectangular bounds of this shape.
        /// </summary>
        public abstract RectangleF Bounds { get; }

        /// <summary>
        /// The brush used to fill this shape.
        /// </summary>
        public Brush Brush { get; set; }

        /// <summary>
        /// The pen used to draw this shape.
        /// </summary>
        public Pen Pen { get; set; }

        /// <summary>
        /// Draws this shape.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// Fills this shape.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public abstract void Fill(Graphics g);

        /// <summary>
        /// Ensures an angle between 0 and 360 degrees is returned.
        /// </summary>
        /// <param name="deg">The angle to normalize.</param>
        /// <returns>The angle adjusted to be between 0 and 360 degrees.</returns>
        protected static float NormalizeDeg(float deg)
        {
            return deg - (float)Math.Floor(deg / 360f) * 360f;
        }

        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        #region public void Dispose()
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        /// <param name="disposing">True to dispose of managed and unmanaged resources.  False to dispose of
        /// unmanaged resources only.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (Pen != null)
                        Pen.Dispose();
                    if (Brush != null)
                        Brush.Dispose();
                }
                IsDisposed = true;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this object is disposed.
        /// </summary>
        /// <value>True if this object is disposed.  Otherwise, false.</value>
        protected bool IsDisposed { get; private set; }
        /// <summary>
        /// Throws an exception if this object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().FullName);
        }
        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        ~Shape()
        {
            Dispose(false);
        }
        #endregion
    }
}
