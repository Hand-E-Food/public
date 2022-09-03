using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColourBlind
{
    public static class ConvertColour
    {
        /// <summary>
        /// Converts hue, saturation and luiminance to a <see cref="Color"/>.
        /// </summary>
        /// <param name="h">The colour's hue from 0 to 360.</param>
        /// <param name="s">The colour's saturation from 0.0 to 1.0.</param>
        /// <param name="l">The colour's luminance from 0.0 to 1.0.</param>
        /// <returns>The <see cref="Color"/> represented by the specified parameters.</returns>
        /// <remarks>Taken from:
        /// https://www.programmingalgorithms.com/algorithm/hsl-to-rgb/
        /// </remarks>
        public static Color FromHSL(double h, double s, double l)
        {
            if (s == 0)
            {
                int i = (int)(255 * l);
                return Color.FromArgb(i, i, i);
            }
            else
            {
                double v2 = l < 0.5
                    ? l * (1 + s)
                    : (l + s) - (l * s);
                
                double v1 = 2 * l - v2;

                h /= 360;

                return Color.FromArgb(
                    (int)(255 * HueToRGB(v1, v2, h + 1.0 / 3.0)),
                    (int)(255 * HueToRGB(v1, v2, h)),
                    (int)(255 * HueToRGB(v1, v2, h - 1.0 / 3.0))
                );
            }
        }

        private static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0)
                vH += 1;

            if (vH > 1)
                vH -= 1;

            if (6 * vH < 1)
                return v1 + (v2 - v1) * 6 * vH;

            if (2 * vH < 1)
                return v2;

            if (3 * vH < 2)
                return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6;

            return v1;
        }
    }
}
