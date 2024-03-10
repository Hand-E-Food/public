// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Rogue
{
    /// <summary>
    /// Stores the location and size of a rectangular region.
    /// </summary>
    [Serializable]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public static readonly Rectangle Empty;

        private int x; // Do not rename (binary serialization)
        private int y; // Do not rename (binary serialization)
        private int width; // Do not rename (binary serialization)
        private int height; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Rectangle'/> class with the specified location
        /// and size.
        /// </summary>
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the location of 0, 0 and the specified size.
        /// </summary>
        public Rectangle(Size size)
        {
            x = 0;
            y = 0;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the specified location and size.
        /// </summary>
        public Rectangle(Point location, Size size)
        {
            x = location.X;
            y = location.Y;
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle class with the specified center and radius.
        /// </summary>
        public Rectangle(Point center, int radius)
        {
            x = center.X - radius;
            y = center.Y - radius;
            width = center.X + radius;
            height = center.Y + radius;
        }

        /// <summary>
        /// Creates a new <see cref='Rectangle'/> with the specified location and size.
        /// </summary>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom) =>
            new Rectangle(left, top, unchecked(right - left), unchecked(bottom - top));

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of the rectangular region represented by this
        /// <see cref='Rectangle'/>.
        /// </summary>
        [Browsable(false)]
        public Point Location
        {
            readonly get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref='Rectangle'/>.
        /// </summary>
        [Browsable(false)]
        public Size Size
        {
            readonly get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Rectangle'/>.
        /// </summary>
        public int X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Rectangle'/>.
        /// </summary>
        public int Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='Rectangle'/>.
        /// </summary>
        public int Width
        {
            readonly get => width;
            set => width = value;
        }

        /// <summary>
        /// Gets or sets the width of the rectangular region defined by this <see cref='Rectangle'/>.
        /// </summary>
        public int Height
        {
            readonly get => height;
            set => height = value;
        }

        /// <summary>
        /// Gets the x-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        [Browsable(false)]
        public readonly int Left => X;

        /// <summary>
        /// Gets the y-coordinate of the upper-left corner of the rectangular region defined by this
        /// <see cref='Rectangle'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Top => Y;

        /// <summary>
        /// Gets the x-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Rectangle'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Right => unchecked(X + Width);

        /// <summary>
        /// Gets the y-coordinate of the lower-right corner of the rectangular region defined by this
        /// <see cref='Rectangle'/>.
        /// </summary>
        [Browsable(false)]
        public readonly int Bottom => unchecked(Y + Height);

        /// <summary>
        /// Gets the center of this rectangle (rounded towards the top-left corner.)
        /// </summary>
        [Browsable(false)]
        public readonly Point Center => new Point(unchecked(x + width / 2), unchecked(y + height / 2));

        /// <summary>
        /// Tests whether this <see cref='Rectangle'/> has a <see cref='Rectangle.Width'/>
        /// or a <see cref='Rectangle.Height'/> of 0.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => height == 0 && width == 0 && x == 0 && y == 0;

        /// <summary>
        /// Returns every <see cref="Point"/> within this <see cref="Rectangle"/>.
        /// </summary>
        public IEnumerable<Point> AllPoints
        {
            get
            {
                for (int y = Top; y < Bottom; y++)
                    for (int x = Left; x < Right; x++)
                        yield return new Point(x, y);
            }
        }

        /// <summary>
        /// Returns every <see cref="Point"/> on this <see cref="Rectangle"/>'s perimiter.
        /// </summary>
        public IEnumerable<Point> AllPointsOnPerimiter
        {
            get
            {
                var points = new List<Point>(Math.Max(1, (width + height - 2) * 2));
                for (int x = Left; x < Right; x++)
                {
                    points.Add(new Point(x, Top));
                    if (Height > 1) points.Add(new Point(x, Bottom - 1));
                }
                for (int y = Top + 1; y < Bottom - 1; y++)
                {
                    points.Add(new Point(Left, y));
                    if (Width > 1) points.Add(new Point(Right - 1, y));
                }
                return points;
            }
        }

        /// <summary>
        /// Tests whether <paramref name="obj"/> is a <see cref='Rectangle'/> with the same location
        /// and size of this Rectangle.
        /// </summary>
        public override readonly bool Equals(object? obj) => obj is Rectangle other && Equals(other);

        public readonly bool Equals(Rectangle other) => this == other;

        /// <summary>
        /// Tests whether two <see cref='Rectangle'/> objects have equal location and size.
        /// </summary>
        public static bool operator ==(Rectangle left, Rectangle right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        /// <summary>
        /// Tests whether two <see cref='Rectangle'/> objects differ in location or size.
        /// </summary>
        public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(int x, int y) => X <= x && x < X + Width && Y <= y && y < Y + Height;

        /// <summary>
        /// Determines if the specified point is contained within the rectangular region defined by this
        /// <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(Point pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines if the rectangular region represented by <paramref name="rect"/> is entirely contained within the
        /// rectangular region represented by this <see cref='Rectangle'/> .
        /// </summary>
        public readonly bool Contains(Rectangle rect) =>
            (X <= rect.X) && (rect.X + rect.Width <= X + Width) &&
            (Y <= rect.Y) && (rect.Y + rect.Height <= Y + Height);

        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        /// <summary>
        /// Inflates this <see cref='Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(int width, int height)
        {
            unchecked
            {
                X -= width;
                Y -= height;

                Width += 2 * width;
                Height += 2 * height;
            }
        }

        /// <summary>
        /// Inflates this <see cref='Rectangle'/> by the specified amount.
        /// </summary>
        public void Inflate(Size size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates a <see cref='Rectangle'/> that is inflated by the specified amount.
        /// </summary>
        public static Rectangle Inflate(Rectangle rect, int x, int y)
        {
            Rectangle r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </summary>
        public void Intersect(Rectangle rect)
        {
            Rectangle result = Intersect(rect, this);

            X = result.X;
            Y = result.Y;
            Width = result.Width;
            Height = result.Height;
        }

        /// <summary>
        /// Creates a rectangle that represents the intersection between a and b. If there is no intersection, an
        /// empty rectangle is returned.
        /// </summary>
        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            int x1 = Math.Max(a.X, b.X);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Max(a.Y, b.Y);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        /// <summary>
        /// Determines if this rectangle intersects with rect.
        /// </summary>
        public readonly bool IntersectsWith(Rectangle rect) =>
            (rect.X < X + Width) && (X < rect.X + rect.Width) &&
            (rect.Y < Y + Height) && (Y < rect.Y + rect.Height);

        /// <summary>
        /// Creates a rectangle that represents the union between a and b.
        /// </summary>
        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            int x1 = Math.Min(a.X, b.X);
            int x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Min(a.Y, b.Y);
            int y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(Point pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Adjusts the location of this rectangle by the specified amount.
        /// </summary>
        public void Offset(int x, int y)
        {
            unchecked
            {
                X += x;
                Y += y;
            }
        }

        /// <summary>
        /// Converts the attributes of this <see cref='Rectangle'/> to a human readable string.
        /// </summary>
        public override readonly string ToString() =>
            "{X=" + X.ToString() + ",Y=" + Y.ToString() +
            ",Width=" + Width.ToString() + ",Height=" + Height.ToString() + "}";
    }
}