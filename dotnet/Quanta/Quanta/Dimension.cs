using System;

namespace Quanta
{
    /// <summary>
    /// Represents a dimension.
    /// </summary>
    public class Dimension
    {

        /// <summary>
        /// This dimension's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the SI unit for this dimension.
        /// </summary>
        public Unit SIUnit => Units.SIUnit;

        /// <summary>
        /// This dimension's available Quanta.
        /// </summary>
        public UnitCollection Units { get; } = new UnitCollection();

        /// <summary>
        /// Gets the <see cref="Unit"/> with the specified abbreviation.
        /// </summary>
        /// <param name="abbreviation">The unit's abbreviation.</param>
        /// <returns>The <see cref="Unit"/> with the specified abbreviation.</returns>
        public Unit this[string abbreviation] => Units[abbreviation];

        /// <summary>
        /// Initialises a new <see cref="Dimension"/>.
        /// </summary>
        /// <param name="name">This dimension's name.</param>
        /// <exception cref="ArgumentNullException">name is null.</exception>
        public Dimension(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
        }
    }
}
