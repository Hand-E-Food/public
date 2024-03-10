// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace Rogue
{
    /// <summary>
    /// Represents the size of a rectangular region with an ordered pair of width and height.
    /// </summary>
    [Serializable]
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class.
        /// </summary>
        public static readonly Size Empty;

        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class from the specified
        /// <see cref='Point'/>.
        /// </summary>
        public Size(Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class from the specified
        /// <see cref='Vector'/>.
        /// </summary>
        public Size(Vector v)
        {
            width = v.I;
            height = v.J;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Size'/> class from the specified dimensions.
        /// </summary>
        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Performs vector addition of two <see cref='Size'/> objects.
        /// </summary>
        public static Size operator +(Size sz1, Size sz2) => Add(sz1, sz2);

        /// <summary>
        /// Contracts a <see cref='Size'/> by another <see cref='Size'/>
        /// </summary>
        public static Size operator -(Size sz1, Size sz2) => Subtract(sz1, sz2);

        /// <summary>
        /// Multiplies a <see cref="Size"/> by an <see cref="int"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplier of type <see cref="int"/>.</param>
        /// <param name="right">Multiplicand of type <see cref="Size"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(int left, Size right) => Multiply(right, left);

        /// <summary>
        /// Multiplies <see cref="Size"/> by an <see cref="int"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Multiplicand of type <see cref="Size"/>.</param>
        /// <param name="right">Multiplier of type <see cref="int"/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        public static Size operator *(Size left, int right) => Multiply(left, right);

        /// <summary>
        /// Divides <see cref="Size"/> by an <see cref="int"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="left">Dividend of type <see cref="Size"/>.</param>
        /// <param name="right">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Size"/>.</returns>
        public static Size operator /(Size left, int right) => new Size(unchecked(left.width / right), unchecked(left.height / right));

        /// <summary>
        /// Tests whether two <see cref='Size'/> objects are identical.
        /// </summary>
        public static bool operator ==(Size sz1, Size sz2) => sz1.Width == sz2.Width && sz1.Height == sz2.Height;

        /// <summary>
        /// Tests whether two <see cref='Size'/> objects are different.
        /// </summary>
        public static bool operator !=(Size sz1, Size sz2) => !(sz1 == sz2);

        /// <summary>
        /// Converts the specified <see cref='Size'/> to a <see cref='Point'/>.
        /// </summary>
        public static explicit operator Point(Size size) => new Point(size.Width, size.Height);

        /// <summary>
        /// Tests whether this <see cref='Size'/> has zero width and height.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => width == 0 && height == 0;

        /// <summary>
        /// Represents the horizontal component of this <see cref='Size'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='Size'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets the center of this size (rounded down.)
        /// </summary>
        [Browsable(false)]
        public Point Center => new Point(unchecked(width / 2), unchecked(height / 2));

        /// <summary>
        /// Performs vector addition of two <see cref='Size'/> objects.
        /// </summary>
        public static Size Add(Size sz1, Size sz2) =>
            new Size(unchecked(sz1.Width + sz2.Width), unchecked(sz1.Height + sz2.Height));

        /// <summary>
        /// Contracts a <see cref='Size'/> by another <see cref='Size'/> .
        /// </summary>
        public static Size Subtract(Size sz1, Size sz2) =>
            new Size(unchecked(sz1.Width - sz2.Width), unchecked(sz1.Height - sz2.Height));

        /// <summary>
        /// Tests to see whether the specified object is a <see cref='Size'/>  with the same dimensions
        /// as this <see cref='Size'/>.
        /// </summary>
        public override readonly bool Equals(object? obj) => obj is Size && Equals((Size)obj);

        public readonly bool Equals(Size other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(Width, Height);

        /// <summary>
        /// Creates a human-readable string that represents this <see cref='Size'/>.
        /// </summary>
        public override readonly string ToString() => "{Width=" + width.ToString() + ", Height=" + height.ToString() + "}";

        /// <summary>
        /// Multiplies <see cref="Size"/> by an <see cref="int"/> producing <see cref="Size"/>.
        /// </summary>
        /// <param name="size">Multiplicand of type <see cref="Size"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref='int'/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        private static Size Multiply(Size size, int multiplier) =>
            new Size(unchecked(size.width * multiplier), unchecked(size.height * multiplier));
    }
}