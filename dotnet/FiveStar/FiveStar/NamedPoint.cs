using System;
using System.Drawing;

namespace FiveStar
{
    public struct NamedPoint
    {
        public string Name;
        public double? Score;
        public double X;
        public double Y;

        public NamedPoint(double x, double y, string name = null)
        {
            X = x;
            Y = y;
            Name = name;
            Score = null;
        }

        public double DistanceTo(NamedPoint point)
        {
            var Δx = X - point.X;
            var Δy = Y - point.Y;
            return Math.Sqrt(Δx * Δx + Δy * Δy);
        }

        public static implicit operator NamedPoint(Point p) => new NamedPoint(p.X, p.Y);
        public static implicit operator NamedPoint(PointF p) => new NamedPoint(p.X, p.Y);

        public static explicit operator PointF(NamedPoint p) => new PointF((float)p.X, (float)p.Y);
    }
}
