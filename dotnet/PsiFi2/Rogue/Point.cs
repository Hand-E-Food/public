// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Rogue
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    [Serializable]
    public struct Point : IEquatable<Point>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='Point'/> class with member data left uninitialized.
        /// </summary>
        public static readonly Point Empty;

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Point'/> class with the specified coordinates.
        /// </summary>
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Point'/> class from a <see cref='Size'/> .
        /// </summary>
        public Point(Size sz)
        {
            x = sz.Width;
            y = sz.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Point'/> class from a <see cref='Vector'/>.
        /// </summary>
        public Point(Vector v)
        {
            x = v.I;
            y = v.J;
        }

        /// <summary>
        /// Initializes a new instance of the Point class using coordinates specified by an integer value.
        /// </summary>
        public Point(int dw)
        {
            x = LowInt16(dw);
            y = HighInt16(dw);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref='Point'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0 && y == 0;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='Point'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='Point'/>.
        /// </summary>
        public int Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Creates a <see cref='Size'/> with the coordinates of the specified <see cref='Point'/>.
        /// </summary>
        public static explicit operator Size(Point p) => new Size(p.X, p.Y);

        /// <summary>
        /// Translates a <see cref='Point'/> by a given <see cref='Vector'/>.
        /// </summary>
        public static Point operator +(Point p, Vector v) => Add(p, v);

        /// <summary>
        /// Translates a <see cref='Point'/> by the negative of a given <see cref='Point'/>.
        /// </summary>
        public static Vector operator -(Point a, Point b) => Subtract(a, b);

        /// <summary>
        /// Translates a <see cref='Point'/> by the negative of a given <see cref='Vector'/>.
        /// </summary>
        public static Point operator -(Point p, Vector v) => Subtract(p, v);

        /// <summary>
        /// Compares two <see cref='Point'/> objects. The result specifies whether the values of the
        /// <see cref='Point.X'/> and <see cref='Point.Y'/> properties of the two
        /// <see cref='Point'/> objects are equal.
        /// </summary>
        public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='Point'/> objects. The result specifies whether the values of the
        /// <see cref='Point.X'/> or <see cref='Point.Y'/> properties of the two
        /// <see cref='Point'/>  objects are unequal.
        /// </summary>
        public static bool operator !=(Point left, Point right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='Point'/> by a given <see cref='Vector'/>.
        /// </summary>
        public static Point Add(Point p, Vector v) => new Point(unchecked(p.X + v.I), unchecked(p.Y + v.J));

        /// <summary>
        /// Calculates the <see cref="Vector"/> from point <paramref name="b"/> to point <paramref name="a"/>.
        /// </summary>
        public static Vector Subtract(Point a, Point b) => new Vector(unchecked(a.X - b.X), unchecked(a.Y - b.Y));

        /// <summary>
        /// Translates a <see cref='Point'/> by the negative of a given <see cref='Vector'/>.
        /// </summary>
        public static Point Subtract(Point p, Vector v) => new Point(unchecked(p.X - v.I), unchecked(p.Y - v.J));

        /// <summary>
        /// Specifies whether this <see cref='Point'/> contains the same coordinates as the specified
        /// <see cref='object'/>.
        /// </summary>
        public override readonly bool Equals(object? obj) => obj is Point && Equals((Point)obj);

        public readonly bool Equals(Point other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        /// <summary>
        /// Translates this <see cref='Point'/> by the specified amount.
        /// </summary>
        public void Offset(int dx, int dy)
        {
            unchecked
            {
                X += dx;
                Y += dy;
            }
        }

        /// <summary>
        /// Translates this <see cref='Point'/> by the specified amount.
        /// </summary>
        public void Offset(Point p) => Offset(p.X, p.Y);

        /// <summary>
        /// Translates this <see cref='Point'/> by the specified amount.
        /// </summary>
        public void Offset(Vector v) => Offset(v.I, v.J);

        /// <summary>
        /// Converts this <see cref='Point'/> to a human readable string.
        /// </summary>
        public override readonly string ToString() => "{X=" + X.ToString() + ",Y=" + Y.ToString() + "}";

        private static short HighInt16(int n) => unchecked((short)((n >> 16) & 0xffff));

        private static short LowInt16(int n) => unchecked((short)(n & 0xffff));
    }
}