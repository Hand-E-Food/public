using System;
using System.Drawing;
using System.Linq;

namespace FiveStar
{
    public class ConstellationPainter : IConstellationPainter, IDisposable
    {
        private const double DetourThreshold = 0.96;
        private const float LargeStarRadius = 6;
        private const float SmallStarRadius = 4;

        private readonly Graphics graphics;

        public Pen LinePen { get; set; } = new Pen(Color.White, 2f);
        public Pen StarPen { get; set; } = new Pen(Color.Black, 2f);
        public Brush StarBrush { get; set; } = new SolidBrush(Color.White);

        public ConstellationPainter(Graphics graphics)
        {
            this.graphics = graphics;
        }

        public void Paint(NamedPoint[] points)
        {
            if (points.Length == 0)
                return;

            points[0].Score = 0;

            for(int i = 1; i < points.Length; i++)
            {
                int bestTarget = 0;
                double bestDistance = points[0].DistanceTo(points[i]);
                for(int j = 1; j < i; j++)
                {
                    var distance = (points[0].DistanceTo(points[j]) + points[i].DistanceTo(points[j])) * DetourThreshold;
                    if (distance < bestDistance)
                    {
                        bestTarget = j;
                        bestDistance = distance;
                    }
                }

                PaintLine(points[bestTarget], points[i]);
            }

            foreach(var point in points.Skip(1))
                PaintStar(point, SmallStarRadius);

            PaintStar(points[0], LargeStarRadius);

        }

        private void PaintLine(NamedPoint origin, NamedPoint point)
        {
            graphics.DrawLine(LinePen, (PointF)origin, (PointF)point);
        }

        private void PaintStar(NamedPoint centre, float radius)
        {
            var bounds = new RectangleF((float)centre.X - radius, (float)centre.Y - radius, radius * 2, radius * 2);
            graphics.FillEllipse(StarBrush, bounds);
            graphics.DrawEllipse(StarPen, bounds);
        }

        private bool isDisposed = false;
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    LinePen?.Dispose();
                    StarPen?.Dispose();
                    StarBrush?.Dispose();
                }
                isDisposed = true;
            }
        }

    }
}
