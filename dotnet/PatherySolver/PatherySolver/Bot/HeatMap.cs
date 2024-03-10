using System.Collections.Generic;

namespace PatherySolver.Bot
{
    /// <summary>
    /// Measures the number of times each cell of a map is walked upon.
    /// </summary>
    public class HeatMap : Array2D<int>
    {
        /// <summary>
        /// Creates a clone from the specified <see cref="HeatMap"/>.
        /// </summary>
        /// <param name="clone">The <see cref="HeatMap"/> to clone.</param>
        protected HeatMap(HeatMap clone) : base(clone)
        { }

        /// <inheritdoc/>
        public HeatMap(int width, int height) : base(width, height)
        { }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public new HeatMap Clone() => new(this);

        /// <summary>
        /// Gets list of the locations that have the highest heat and changes their heat to 0.
        /// </summary>
        /// <returns>All locations that have the highest heat.</returns>
        public List<Location> PopHottestLocations()
        {
            var result = new List<Location>();
            var hottest = 1;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var heat = this[x, y];
                    if (hottest <= heat)
                    {
                        if (hottest < heat)
                        {
                            hottest = heat;
                            result.Clear();
                        }
                        result.Add(new Location(x, y));
                    }
                }
            }

            foreach (var location in result)
                this[location] = 0;

            return result;
        }
    }
}
