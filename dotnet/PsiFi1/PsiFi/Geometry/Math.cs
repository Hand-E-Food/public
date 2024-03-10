using PsiFi.Models;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace PsiFi.Geometry
{
    static class Math
    {
        /// <summary>
        /// Calculates the visibility of a cell given the specified visibility modes and map.
        /// </summary>
        /// <param name="cell">The target cell.</param>
        /// <param name="visions">Teh visibility modes.</param>
        /// <param name="map">The map.</param>
        /// <returns>A value indicating how visible the cell is to the specified visibility modes.</returns>
        public static CellVisibility CalculateVisibility(Cell cell, ICollection<Vision> visions, Map map)
        {
            var cellLocation = cell.Location;

            var visible = visions
                .Where(vision => IsWithinRange(vision.Distance, vision.Origin, cellLocation))
                .Any(vision => GetStraightPath(vision.Origin, cellLocation)
                    .Select(point => map[point])
                    .All(cell => vision.CanPermeate(cell))
                );

            return visible ? CellVisibility.Visible : CellVisibility.NotVisible;
        }

        /// <summary>
        /// Gets a straight path between two points.
        /// </summary>
        /// <param name="origin">The origin point.</param>
        /// <param name="target">The target point.</param>
        /// <returns>The cells that lie in a stright line between the two points, not including the two points.</returns>
        public static IEnumerable<Point> GetStraightPath(Point origin, Point target)
        {
            if (origin == target) yield break;

            double xExact = origin.X;
            double yExact = origin.Y;
            double xStep = target.X - origin.X;
            double yStep = target.Y - origin.Y;
            double ratio = Abs(xStep) > Abs(yStep)
                ? Abs(xStep)
                : Abs(yStep);
            xStep /= ratio;
            yStep /= ratio;

            while(true)
            {
                xExact += xStep;
                yExact += yStep;
                var point = new Point((int)Round(xExact), (int)Round(yExact));
                if (point == target) yield break;
                yield return point;
            }
        }

        /// <summary>
        /// Determines whether two points are within the specified <paramref name="range"/> of each other.
        /// </summary>
        /// <param name="range">The maximum range.</param>
        /// <param name="origin">The origin point.</param>
        /// <param name="target">The target point.</param>
        /// <returns>True if the distance between the two points is within the <paramref name="range"/>. Otherwise, false.</returns>
        public static bool IsWithinRange(int range, Point origin, Point target)
        {
            var x = Abs(origin.X - target.X);
            var y = Abs(origin.Y - target.Y);
            return x <= range
                && y <= range
                && (int)Ceiling(Sqrt(x * x + y * y)) <= range;
        }
    }
}
