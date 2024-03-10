using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FiveStar.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("images/Australia");
            Directory.CreateDirectory("images/Brazil");

            CreateStarChart(Predefined.Australia.AllCapitals, "images/Australia/All Capitals.png");
            CreateStarChart(Predefined.Australia.NewSouthWales, "images/Australia/New South Wales.png");
            CreateStarChart(Predefined.Australia.NorthernTerritory, "images/Australia/Northern Territory.png");
            CreateStarChart(Predefined.Australia.Queensland, "images/Australia/Queensland.png");
            CreateStarChart(Predefined.Australia.SouthAustralia, "images/Australia/South Australia.png");
            CreateStarChart(Predefined.Australia.Tasmania, "images/Australia/Tasmania.png");
            CreateStarChart(Predefined.Australia.Victoria, "images/Australia/Victoria.png");
            CreateStarChart(Predefined.Australia.WesternAustralia, "images/Australia/Western Australia.png");

            CreateStarChart(Predefined.Brazil.RioGrandeDoNorte, "images/Brazil/Rio Grande do Norte.png");
        }

        private static void CreateStarChart(IEnumerable<SphericalPoint> locations, string Path)
        {
            var spericalPoints = locations.ToArray();
            var projector = new SphericalProjector(spericalPoints.First());
            //var projector = new EquirectangularProjector(spericalPoints.First());
            var rawPoints = locations.Select(sphericalPoint => projector.Project(sphericalPoint));
            using (var image = new Bitmap(200, 200))
            using (var graphics = Graphics.FromImage(image))
            {
                //graphics.Clear(Color.Black);
                var points = Fit(image, rawPoints.ToList()).ToArray();
                points = points.OrderBy(p => p.DistanceTo(points[0])).ToArray();
                var painter = new ConstellationPainter(graphics);
                painter.Paint(points);
                image.Save(Path, ImageFormat.Png);
            }
        }

        private static IEnumerable<NamedPoint> Fit(Bitmap image, IList<NamedPoint> points)
        {
            var source = new RectangleD();
            source.X = points.Min(p => p.X);
            source.Y = points.Min(p => p.Y);
            source.Width  = points.Max(p => p.X) - source.X;
            source.Height = points.Max(p => p.Y) - source.Y;

            const double Margin = 0.05;
            var target = new RectangleD(0, 0, image.Width, image.Height);
            target.X += target.Width  * Margin;
            target.Y += target.Height * Margin;
            target.Width  *= 1.0 - Margin * 2.0;
            target.Height *= 1.0 - Margin * 2.0;

            var ratio = (source.Width / source.Height) / (target.Width / target.Height);
            if (ratio > 1.0)
            {
                source.Y -= source.Height * (ratio / 2.0 - 0.5);
                source.Height *= ratio;
            }
            else if (ratio < 1.0)
            {
                ratio = 1.0 / ratio;
                source.X -= source.Width * (ratio / 2.0 - 0.5);
                source.Width *= ratio;
            }

            return points.Select(point => new NamedPoint(
                (point.X - source.X) / source.Width  * target.Width  + target.X,
                (point.Y - source.Y) / source.Height * target.Height + target.Y,
                point.Name
            ));
        }
    }
}
