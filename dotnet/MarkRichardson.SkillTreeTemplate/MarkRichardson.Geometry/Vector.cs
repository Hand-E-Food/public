using System;
using System.Drawing;
using System.Linq;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// Provides methods for calculating vectors.
    /// </summary>
    public static class Vector
    {

        /// <summary>
        /// Gets the angle, in degrees from one point to another.
        /// </summary>
        /// <param name="origin">The origin point.</param>
        /// <param name="target">The target point.</param>
        /// <returns>The angle from the origin point to the target point in degrees.</returns>
        public static float AngleTo(PointF origin, PointF target)
        {
            var pi = (float)Math.PI;
            var dx = target.X - origin.X;
            var dy = target.Y - origin.Y;

            if (dy == 0f)
                return Math.Sign(dx) * 90f;
            else if (dx == 0f)
                return dy > 0f ? 0f : 180f;
            else
                return (float)RadToDeg(Math.Atan(dx / dy) + (dy > 0 ? 0f : pi));
        }

        /// <summary>
        /// Calculates the average of a set of points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The average of the specified points.</returns>
        public static PointF Average(params PointF[] points)
        {
            return new PointF(points.Average(p => p.X), points.Average(p => p.Y));
        }

        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="deg">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static double DegToRad(double deg)
        {
            const double Ratio = Math.PI / 180.0;
            return deg * Ratio;
        }
        /// <summary>
        /// Converts an angle from degrees to radians.
        /// </summary>
        /// <param name="deg">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        public static float DegToRad(float deg)
        {
            const float Ratio = (float)(Math.PI / 180.0);
            return deg * Ratio;
        }

        /// <summary>
        /// Returns the point that is at a polar coordinate.
        /// </summary>
        /// <param name="origin">The polar origin.</param>
        /// <param name="deg">The direction of the point to return in degrees.</param>
        /// <param name="r">The radius of the point to return.</param>
        /// <returns>The point r units away from origin in the direction of deg.</returns>
        public static PointF Polar(PointF origin, float deg, float r)
        {
            var rad = DegToRad(deg);
            return new PointF((float)(origin.X + Math.Sin(rad) * r), (float)(origin.Y + Math.Cos(rad) * r));
        }

        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="rad">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static double RadToDeg(double deg)
        {
            const double Ratio = 180.0 / Math.PI;
            return deg * Ratio;
        }
        /// <summary>
        /// Converts an angle from radians to degrees.
        /// </summary>
        /// <param name="rad">The angle in radians.</param>
        /// <returns>The angle in degrees.</returns>
        public static float RadToDeg(float deg)
        {
            const float Ratio = (float)(180.0 / Math.PI);
            return deg * Ratio;
        }
    }
}
