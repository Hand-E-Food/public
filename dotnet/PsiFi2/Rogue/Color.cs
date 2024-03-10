using System;
using System.Collections.Generic;

namespace Rogue
{
    public struct Color : IEquatable<Color>
    {
        public readonly uint Argb;
        public readonly byte A;
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public Color(uint argb)
        {
            if (argb <= 0xffff)
                argb = ((argb & 0xf000) << 16)
                     | ((argb & 0xff00) << 12)
                     | ((argb & 0x0ff0) <<  8)
                     | ((argb & 0x00ff) <<  4)
                     |  (argb & 0x000f);

            Argb = argb;
            A = (byte)((argb >> 24) & 0xff);
            R = (byte)((argb >> 16) & 0xff);
            G = (byte)((argb >>  8) & 0xff);
            B = (byte)( argb        & 0xff);
        }

        public Color(byte r, byte g, byte b)
        {
            A = 0xff;
            R = r;
            G = g;
            B = b;
            Argb = (((uint)A) << 24)
                 | (((uint)R) << 16)
                 | (((uint)G) <<  8)
                 |   (uint)B;
        }

        public string ToVT100ForegroundString() => $"\x1b[38;2;{R};{G};{B}m";

        public string ToVT100BackgroundString() => $"\x1b[48;2;{R};{G};{B}m";

        public override bool Equals(object obj) => obj is Color other && Equals(other);

        public bool Equals(Color other) => Argb == other.Argb;

        public override int GetHashCode() => Argb.GetHashCode();

        public static bool operator ==(Color left, Color right) => left.Equals(right);

        public static bool operator !=(Color left, Color right) => !left.Equals(right);

        public static implicit operator Color(uint argb) => new Color(argb);

        public static implicit operator Color(ConsoleColor color) => consoleColors[color];

        private static readonly Dictionary<ConsoleColor, Color> consoleColors = new Dictionary<ConsoleColor, Color>
        {
            { ConsoleColor.Black, 0xff000000 },
            { ConsoleColor.DarkBlue, 0xff000080 },
            { ConsoleColor.DarkGreen, 0xff008000 },
            { ConsoleColor.DarkCyan, 0xff008080 },
            { ConsoleColor.DarkRed, 0xff800000 },
            { ConsoleColor.DarkMagenta, 0xff800080 },
            { ConsoleColor.DarkYellow, 0xff808000 },
            { ConsoleColor.Gray, 0xffc0c0c0 },
            { ConsoleColor.DarkGray, 0xff808080 },
            { ConsoleColor.Blue, 0xff0000ff },
            { ConsoleColor.Green, 0xff00ff00 },
            { ConsoleColor.Cyan, 0xff00ffff },
            { ConsoleColor.Red, 0xffff0000 },
            { ConsoleColor.Magenta, 0xffff00ff },
            { ConsoleColor.Yellow, 0xffffff00 },
            { ConsoleColor.White, 0xffffffff },
        };

        public static readonly Color Black = new Color(0xf000);
        public static readonly Color Blue = new Color(0xf00f);
        public static readonly Color Cyan = new Color(0xf0ff);
        public static readonly Color Gray = new Color(0xfccc);
        public static readonly Color Green = new Color(0xf0f0);
        public static readonly Color Magenta = new Color(0xff0f);
        public static readonly Color Orange = new Color(0xff80);
        public static readonly Color Purple = new Color(0xf808);
        public static readonly Color Red = new Color(0xff00);
        public static readonly Color Yellow = new Color(0xfff0);
        public static readonly Color White = new Color(0xffff);
    }
}
