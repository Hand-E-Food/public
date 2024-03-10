using System;

namespace PatherySolver
{
    /// <summary>
    /// A 2-dimensional array with specialised methods.
    /// </summary>
    /// <typeparam name="T">The type of objects contained in this array.</typeparam>
    public class Array2D<T> : ICloneable
    {
        /// <summary>
        /// The underlying array.
        /// </summary>
        public T[,] Array { get; }

        /// <summary>
        /// This array's height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The array's width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Creates a clone from the specified <see cref="Array2D{T}"/>.
        /// </summary>
        /// <param name="clone">The <see cref="Array2D{T}"/> to clone.</param>
        protected Array2D(Array2D<T> clone)
        {
            Array = (T[,])clone.Array.Clone();
            Height = clone.Height;
            Width = clone.Width;
        }

        /// <summary>
        /// Initialises a new 2 dimensional array.
        /// </summary>
        /// <param name="width">The array's width.</param>
        /// <param name="height">The array's height.</param>
        public Array2D(int width, int height)
        {
            Array = new T[width, height];
            Height = height;
            Width = width;
        }

        /// <summary>
        /// The value at the specified coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public T this[int x, int y]
        {
            get => Array[x, y];
            set => Array[x, y] = value;
        }

        /// <summary>
        /// The value at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public T this[Location location]
        {
            get => Array[location.X, location.Y];
            set => Array[location.X, location.Y] = value;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public Array2D<T> Clone() => new(this);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Resets every cell in this array to the default value.
        /// </summary>
        public virtual void Reset() => Reset(default);

        /// <summary>
        /// Resets every cell in this array to the specified value.
        /// </summary>
        /// <param name="value">The value to set.
        public void Reset(T value)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Array[x, y] = value;
        }
    }
}
