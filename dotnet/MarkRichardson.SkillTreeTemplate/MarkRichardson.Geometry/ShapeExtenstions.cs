using System.Collections.Generic;
using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// Contains extenstion methods for <see cref="Shape"/> objects.
    /// </summary>
    public static class ShapeExtenstions
    {

        /// <summary>
        /// Draws all shapes in this collection.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public static void Draw(this IEnumerable<Shape> Values, Graphics g)
        {
            foreach (var value in Values)
            {
                value.Draw(g);
            }
        }

        /// <summary>
        /// Fills all shapes in this collection.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public static void Fill(this IEnumerable<Shape> Values, Graphics g)
        {
            foreach (var value in Values)
            {
                value.Fill(g);
            }
        }

        /// <summary>
        /// Fills and draws all shapes in this collection.
        /// </summary>
        /// <param name="g">The graphics to fill and draw to.</param>
        public static void FillAndDraw(this IEnumerable<Shape> Values, Graphics g)
        {
            foreach (var value in Values)
            {
                value.Fill(g);
                value.Draw(g);
            }
        }
    }
}
