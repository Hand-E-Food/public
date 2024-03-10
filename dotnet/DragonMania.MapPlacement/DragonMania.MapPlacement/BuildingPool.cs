using System;
using System.Collections.Generic;

namespace DragonMania.MapPlacement
{
    /// <summary>
    /// A pool of buildings to be added to the map.
    /// </summary>
    public class BuildingPool : ICloneable
    {
        /// <summary>
        /// The size of the buildings in this pool
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The number of buildings in this pool.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// True if these buildings are required; otherwise false.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Returns an exact copy of this object.
        /// </summary>
        /// <returns>An exact copy of this object.</returns>
        public BuildingPool Clone() => (BuildingPool)MemberwiseClone();
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Returns a building from this pool and reduces the <see cref="Count"/> by 1.
        /// </summary>
        /// <returns>A building from this pool.</returns>
        public Building Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException("This building pool is empty.");

            Count--;
            return new Building(Size, Required);
        }

        /// <summary>
        /// Returns all buildings from this pool and reduces the <see cref="Count"/> to 0.
        /// </summary>
        /// <returns>All buildings in this pool.</returns>
        public IEnumerable<Building> PopAll()
        {
            while (Count > 0)
                yield return Pop();
        }
    }
}
