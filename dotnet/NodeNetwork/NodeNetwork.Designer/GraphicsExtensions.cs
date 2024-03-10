using System.Drawing;

namespace NodeNetwork.Designer
{
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Draws the specified text string in the specified rectangle with the specified <see cref="TextStyle"/> object.
        /// </summary>
        /// <param name="g">The GDI+ drawing surface.</param>
        /// <param name="s">String to draw.</param>
        /// <param name="textFormat">The presentation options.</param>
        /// <param name="layoutRectangle"><see cref="RectangleF"/> structure that specifies the location of the drawn text.</param>
        public static void DrawString(this Graphics g, string s, TextStyle textFormat, RectangleF layoutRectangle) =>
            g.DrawString(s, textFormat.Font, textFormat.Brush, layoutRectangle, textFormat.StringFormat);

        /// <summary>
        /// Draws the specified text string at the specified location with the specified <see cref="TextStyle"/> object.
        /// </summary>
        /// <param name="g">The GDI+ drawing surface.</param>
        /// <param name="s">String to draw.</param>
        /// <param name="textFormat">The presentation options.</param>
        /// <param name="point"><see cref="PointF"/> structure that specifies the upper-left corner of the drawn text.</param>
        public static void DrawString(this Graphics g, string s, TextStyle textFormat, PointF point) =>
            g.DrawString(s, textFormat.Font, textFormat.Brush, point, textFormat.StringFormat);

        /// <summary>
        /// Draws the specified text string at the specified location with the specified <see cref="TextStyle"/> object.
        /// </summary>
        /// <param name="g">The GDI+ drawing surface.</param>
        /// <param name="s">String to draw.</param>
        /// <param name="textFormat">The presentation options.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the drawn text.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the drawn text.</param>
        public static void DrawString(this Graphics g, string s, TextStyle textFormat, float x, float y) =>
            g.DrawString(s, textFormat.Font, textFormat.Brush, x, y, textFormat.StringFormat);
    }
}
