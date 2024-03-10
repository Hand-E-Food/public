using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Metro.Delayed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var image = CreateImage();
            BackgroundImage = image;
            image.Save("Delayed.bmp", ImageFormat.Bmp);
        }

        private Image CreateImage()
        {
            var image = new Bitmap(160, 100);
            using (var g = Graphics.FromImage(image))
            {
                g.CompositingQuality = CompositingQuality.HighSpeed;

                // Background
                g.Clear(Color.Black);

                // Lines
                var lines = new[]
                {
                    new[] { P(40, 12), P(36, 12), P(35, 11) }, // Central top
                    new[] { P(40, 13), P(36, 13), P(35, 14) }, // Central bottom
                    new[] { P(40, 12), P(37, 12), P(26, 23), P( 2, 23) }, // South
                    new[] { P(40, 13), P(37, 13), P(26,  2), P( 2,  2) }, // North
                    new[] { P(31,  7), P( 2,  7) }, // Mid-North
                    new[] { P(25,  7), P(20, 12), P( 2, 12) }, // Mid
                    new[] { P(31, 18), P( 2, 18) }, // Mid-South
                    new[] { P(26, 18), P(25, 18), P(24, 17), P(10, 17), P( 9, 18), P( 8, 18) }, // Siding
                };

                using (var linePen = GetLinePen())
                    foreach (var line in lines)
                        g.DrawLines(linePen, line);

                // Signals
                var signals = new[] {
                    R(39, 12), // Central
                    R(39, 13), // Central
                    R(34, 10), // Central -> North
                    R(28,  4), // North
                    R(27,  3), // North
                    R(22,  2), // North
                    R(21,  2), // North
                    R(16,  2), // North
                    R(15,  2), // North
                    R(10,  2), // North
                    R( 9,  2), // North
                    R( 4,  2), // North
                    R(28,  7), // Mid-North
                    R(27,  7), // Mid-North
                    R(22,  7), // Mid-North
                    R(21,  7), // Mid-North
                    R(16,  7), // Mid-North
                    R(15,  7), // Mid-North
                    R(10,  7), // Mid-North
                    R( 5,  7), // Mid-North
                    R(22, 10), // Mid
                    R(21, 11), // Mid
                    R(16, 12), // Mid
                    R(11, 12), // Mid
                    R( 6, 12), // Mid
                    R( 5, 12), // Mid
                    R(34, 15), // Central -> South
                    R(33, 16), // Central -> South
                    R(28, 21), // South
                    R(27, 22), // South
                    R(22, 23), // South
                    R(21, 23), // South
                    R(16, 23), // South
                    R(11, 23), // South
                    R( 6, 23), // South
                    R(28, 18), // Mid-South
                    R(27, 18), // Mid-South
                    R(22, 18), // Siding
                    R(22, 17), // Siding
                    R(17, 18), // Siding
                    R(17, 17), // Siding
                    R(12, 18), // Siding
                    R(12, 17), // Siding
                    R( 7, 18), // Mid-South
                };
                const float RedHue = 0f;
                const float GreenHue = 120f;
                const float Saturation = 1f;
                const float Brightness = 0.7f;
                using (var redBrush = new SolidBrush(ColorFromAhsb(RedHue, Saturation, Brightness)))
                    foreach (var signal in signals)
                        g.FillRectangle(redBrush, signal);
                using (var greenBrush = new SolidBrush(ColorFromAhsb(GreenHue, Saturation, Brightness)))
                    g.FillRectangle(greenBrush, R(33, 9));
            }
            // Enlarge
            image = new Bitmap(image, image.Width * 2, image.Height * 2);
            using (var g = Graphics.FromImage(image))
            {
                // Raster
                for (int y = 1; y < image.Height; y += 2)
                    g.DrawLine(Pens.Black, 0, y, image.Width, y);

                // Text
                using (var textImage = Image.FromFile("Text.bmp"))
                    g.DrawImage(textImage, 0, image.Height - textImage.Height, textImage.Width, textImage.Height);
            }
            return image;
        }

        private static Pen GetLinePen()
        {
            return new Pen(Color.White, 2)
            {
                StartCap = LineCap.Square,
                EndCap = LineCap.Square,
                LineJoin = LineJoin.Miter,
            };
        }

        private Point P(int x, int y) => new Point(x * 4 - 2, y * 4 - 2);

        private Rectangle R(int x, int y) => new Rectangle(x * 4 - 4, y * 4 - 4, 4, 4);

        public static Color ColorFromAhsb(float h, float s, float b) => ColorFromAhsb(255, h, s, b);
        public static Color ColorFromAhsb(int a, float h, float s, float b)
        {
            if (0 > a || 255 < a)
                throw new ArgumentOutOfRangeException(nameof(a), a, "Alpha must be between 0 and 255.");
            if (0f > h || 360f < h)
                throw new ArgumentOutOfRangeException(nameof(h), h, "Hue must be between 0 and 360.");
            if (0f > s || 1f < s)
                throw new ArgumentOutOfRangeException(nameof(s), s, "Saturation must be between 0 and 1.");
            if (0f > b || 1f < b)
                throw new ArgumentOutOfRangeException(nameof(b), b, "Brightness must be between 0 and 1.");

            if (0 == s)
                return Color.FromArgb(a, (int)(b * 255), (int)(b * 255), (int)(b * 255));

            float fMax, fMid, fMin;
            int sextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            sextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
                h -= 360f;
            h /= 60f;
            h -= 2f * (float)Math.Floor(((sextant + 1f) % 6f) / 2f);
            if (0 == sextant % 2)
                fMid = h * (fMax - fMin) + fMin;
            else
                fMid = fMin - h * (fMax - fMin);

            iMax = (int)(fMax * 255);
            iMid = (int)(fMid * 255);
            iMin = (int)(fMin * 255);

            switch (sextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }
    }
}
