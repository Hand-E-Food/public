using System.Collections.Generic;
using System.Drawing;

namespace MarkRichardson.Geometry
{

    /// <summary>
    /// A collection of shapes indexed by unique identifiers.
    /// </summary>
    public class ShapeCollection<T> : Dictionary<string, T> where T : Shape
    {

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <typeparam name="T2">The type to return.</typeparam>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
        /// <returns>value</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the
        /// <see cref="ShapeCollection"/>.</exception>
        public new T2 Add<T2>(string key, T2 value) where T2 : T
        {
            base.Add(key, value);
            return value;
        }

        /// <summary>
        /// Removes all keys and values and disposes all values from the <see cref="ShapeCollection"/>.
        /// </summary>
        public new void Clear()
        {
            foreach (var value in Values)
            {
                if (value != null)
                    value.Dispose();
            }
            base.Clear();
        }

        /// <summary>
        /// Draws all shapes in this collection.
        /// </summary>
        /// <param name="g">The graphics to draw to.</param>
        public void Draw(Graphics g)
        {
            foreach (var value in Values)
            {
                value.Draw(g);
            }
        }

        /// <summary>
        /// Fills all shapes in this collection.
        /// </summary>
        /// <param name="g">The graphics to fill to.</param>
        public void Fill(Graphics g)
        {
            foreach (var value in Values)
            {
                value.Fill(g);
            }
        }

        /// <summary>
        /// Fills and draws all shapes in this collection.
        /// </summary>
        /// <param name="g">The graphics to fill and draw to.</param>
        public void FillAndDraw(Graphics g)
        {
            foreach (var value in Values)
            {
                value.Fill(g);
                value.Draw(g);
            }
        }

        /// <summary>
        /// Removes and disposes the value with the specified key from the <see cref="ShapeCollection"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false.  This method
        /// returns false if key is not found in the <see cref="ShapeCollection"/>.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public new bool Remove(string key)
        {
            T value;
            if (!TryGetValue(key, out value))
                return false;

            if (value != null)
                value.Dispose();

            return base.Remove(key);
        }
    }
}
