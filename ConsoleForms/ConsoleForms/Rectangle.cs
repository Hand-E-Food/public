namespace ConsoleForms
{
    public struct Rectangle
    {
        /// <summary>
        /// An empty rectangle with all coordinates at 0.
        /// </summary>
        public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

        /// <summary>
        /// This rectangle's left edge.
        /// </summary>
        public int Left;

        /// <summary>
        /// This rectangle's top edge.
        /// </summary>
        public int Top;

        /// <summary>
        /// This rectangle's width.
        /// </summary>
        public int Width => Right - Left;

        /// <summary>
        /// This rectangle's height.
        /// </summary>
        public int Height => Bottom - Top;

        /// <summary>
        /// This rectangle's right edge.
        /// </summary>
        public int Right;

        /// <summary>
        /// This rectangle's bototm edge.
        /// </summary>
        public int Bottom;

        /// <summary>
        /// True if this rectangle has width and height. False if this rectangle has zero width or height.
        /// </summary>
        public bool HasArea => Width != 0 && Height != 0;

        /// <summary>
        /// Creates a new <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="x">This rectangle's x position.</param>
        /// <param name="y">This rectangle's y position.</param>
        /// <param name="width">This rectangle's width.</param>
        /// <param name="height">This rectangle's height.</param>
        /// <returns>The new rectangle.</returns>
        public static Rectangle XYWH(int x, int y, int width, int height)
        {
            if (width >= 0)
            {
                if (height >= 0)
                    return new(x, y, width, height);
                else
                    return new(x, y - height, width, -height);
            }
            else
            {
                if (height >= 0)
                    return new(x - width, y, -width, height);
                else
                    return new(x - width, y - height, -width, -height);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="left">This rectangle's left edge.</param>
        /// <param name="top">This rectangle's top edge.</param>
        /// <param name="right">This rectangle's right edge.</param>
        /// <param name="bottom">This rectangle's bottom edge.</param>
        /// <returns>The new rectangle.</returns>
        public static Rectangle LTRB(int left, int top, int right, int bottom)
        {
            if (left <= right)
            {
                if (top <= bottom)
                    return new(left, right, top, bottom);
                else
                    return new(left, right, bottom, top);
            }
            else
            {
                if (top <= bottom)
                    return new(right, left, top, bottom);
                else
                    return new(right, left, bottom, top);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> that contains all of the specified rectangles.
        /// </summary>
        /// <param name="rectangles">The rectangles to contain.</param>
        /// <returns>The new rectangle.</returns>
        public static Rectangle Union(params Rectangle[] rectangles)
        {
            return LTRB(
                rectangles.Min(r => r.Left),
                rectangles.Min(r => r.Top),
                rectangles.Max(r => r.Right),
                rectangles.Max(r => r.Bottom)
            );
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/> that can be contained by all specified rectangles.
        /// </summary>
        /// <param name="rectangles">The rectangles to intersect.</param>
        /// <returns>The new rectangle. If the <paramref name="rectangles"/> do not intersect, returns null.</returns>
        public static Rectangle? Intersection(params Rectangle[] rectangles)
        {
            int left = rectangles.Max(rectangles => rectangles.Left);
            int top = rectangles.Max(rectangles => rectangles.Top);
            int right = rectangles.Min(rectangles => rectangles.Right);
            int bottom = rectangles.Min(rectangles => rectangles.Bottom);
            return left <= right && top <= bottom
                ? LTRB(left, top, right, bottom)
                : null;
        }

        /// <summary>
        /// Creates a new <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="left">This rectangle's left edge.</param>
        /// <param name="top">This rectangle's top edge.</param>
        /// <param name="right">This rectangle's right edge.</param>
        /// <param name="bottom">This rectangle's bottom edge.</param>
        private Rectangle(int left, int right, int top, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Tests if the <paramref name="other"/> rectangle intersects with this rectangle.
        /// </summary>
        /// <param name="other">Thew other rectangle.</param>
        /// <returns>True if the rectangles overlap or touch. Otherwise, false.</returns>
        public bool Intersects(Rectangle other)
        {
            return this.Left < other.Right
                && other.Left < this.Right
                && this.Top < other.Bottom
                && other.Top < this.Bottom;
        }

        /// <summary>
        /// Tests whether the <paramref name="other"/> rectangle fits entirely inside this rectangle.
        /// </summary>
        /// <param name="other">The other rectangle.</param>
        /// <returns>True of the <paramref name="other"/> rectangle fits entirely inside this rectangle. Otherwise, false.</returns>
        public bool Contains(Rectangle other)
        {
            return this.Left <= other.Left
                && other.Right <= this.Right
                && this.Top <= other.Top
                && other.Bottom <= this.Bottom;
        }

        /// <summary>
        /// Tests whether the <paramref name="point"/> is inside this rectangle.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True of the <paramref name="point"/> is inside this rectangle. Otherwise, false.</returns>
        public bool Contains(Point point)
        {
            return this.Left <= point.X
                && point.X < this.Right
                && this.Top <= point.Y
                && point.Y < this.Bottom;
        }
    }
}