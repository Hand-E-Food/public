using System;

namespace DragonMania.MapPlacement
{
    /// <summary>
    /// A building to add to a map.
    /// </summary>
    public class Building : ICloneable
    {
        /// <summary>
        /// Gets this building's size.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets whether this building is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Gets or sets this building's left coordinate.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets this building's right coordinate.
        /// </summary>
        public int Right => Left + Size - 1;

        /// <summary>
        /// Gets or sets this building's top coordinate.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets this building's bottom coordinate.
        /// </summary>
        public int Bottom => Top + Size - 1;

        /// <summary>
        /// Initialises a new building.
        /// </summary>
        /// <param name="size">The building's size.</param>
        /// <param name="required">True if this building is required; otherwise false.</param>
        public Building(int size, bool required)
        {
            Size = size;
            Required = required;
        }

        /// <summary>
        /// Returns an exact copy of this object.
        /// </summary>
        /// <returns>An exact copy of this object.</returns>
        public Building Clone() => (Building)MemberwiseClone();
        object ICloneable.Clone() => Clone();
    }
}
