using System.Collections.Generic;
using System.Linq;
using static Rogue.RogueMath;
using static System.Math;

namespace Rogue
{
    public static class Shape
    {
        public static bool Contains(this Size size, Point point) =>
            point.X >= 0 && point.X < size.Width && point.Y >= 0 && point.Y < size.Height;

        public static IEnumerable<Point> GetPointsInCircle(Point origin, int radius)
        {
            var radiusSquared = RadiusSquared(radius);
            return new Rectangle(origin, radius)
                .AllPoints
                .Where(point => (origin - point).RadiusSquared <= radiusSquared);
        }

        public static IEnumerable<Point> GetPointsInCircle(Point origin, int radius, Rectangle crop)
        {
            var radiusSquared = RadiusSquared(radius);
            var square = new Rectangle(origin, radius);
            square.Intersect(crop);
            return square.AllPoints.Where(point => (origin - point).RadiusSquared <= radiusSquared);
        }

        public static IEnumerable<Point> GetPointsInCircleBorder(Point origin, int radius)
        {
            if (radius == 0)
                return new Point[] { origin };
            if (radius < 0)
                return new Point[0];

            int radiusSquared = RadiusSquared(radius);
            var vector = new Vector(radius, 0);
            var points = new List<Point>(7 * radius); // Ceiling(2 * Pi) * radius
            points.AddRange(vector.MirroredAcrossDiagonalAxes().Select(v => origin + v));

            while (vector.J < vector.I)
            {
                vector.J++;
                if (vector.RadiusSquared > radiusSquared)
                    vector.I--;

                points.AddRange(vector.MirroredAcrossCardinalAndDiagonalAxes().Select(v => origin + v));
            }

            return points;
        }

        public static IEnumerable<Point> GetPointsInLine(Point origin, Point destination)
        {
            var vector = destination - origin;
            int dx = Abs(vector.I);
            int dy = Abs(vector.J);
            int sx = Sign(vector.I);
            int sy = Sign(vector.J);
            int err = dx - dy;

            while (true)
            {
                yield return origin;
                if (origin == destination)
                    break;
                
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    origin.X += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    origin.Y += sy;
                }
            }
        }
    }
}
