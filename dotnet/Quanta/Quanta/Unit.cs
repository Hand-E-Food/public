using System;

namespace Quanta
{
    /// <summary>
    /// Represents a unit of a dimension.
    /// </summary>
    public class Unit
    {
        //TODO: Unit[double, int]
        //public Quantum this[double value, int power = 1] => new Quantum(value, this, power);

        /// <summary>
        /// The dimension this unit measures.
        /// </summary>
        public Dimension Dimension { get; private set; }

        /// <summary>
        /// This unit's full, singular name.
        /// </summary>
        public string SingularName { get; private set; }

        /// <summary>
        /// This unit's full, singular name.
        /// </summary>
        public string PluralName { get; private set; }

        /// <summary>
        /// This unit's displayed abbreviation.
        /// </summary>
        public string Abbreviation { get; private set; }

        /// <summary>
        /// This unit's ratio to the dimension's SI unit.
        /// </summary>
        public double RatioToSI { get; private set; }

        /// <summary>
        /// Initialises a new <see cref="Unit"/> and attaches it the the specified dimension.
        /// </summary>
        /// <param name="dimension">The dimension this unit measures.</param>
        /// <param name="singularName">This unit's full, singular name.</param>
        /// <param name="pluralName">This unit's full, plural name.</param>
        /// <param name="abbreviation">This unit's display abbreviation.</param>
        /// <param name="ratioToSI">This unit's ratio to the dimension's SI unit.</param>
        /// <exception cref="ArgumentNullException">A parameter is null.</exception>
        /// <exception cref="ArgumentException">ratioToSI is zero.</exception>
        public Unit(Dimension dimension, string abbreviation, string singularName, string pluralName, double ratioToSI)
        {
            if (dimension == null)
                throw new ArgumentNullException("dimension");
            if (singularName == null)
                throw new ArgumentNullException("singularName");
            if (pluralName == null)
                throw new ArgumentNullException("pluralName");
            if (abbreviation == null)
                throw new ArgumentNullException("abbreviation");
            if (ratioToSI == 0)
                throw new ArgumentException("ratioToSI is zero.", "ratioToSI");

            Dimension = dimension;
            SingularName = singularName;
            PluralName = pluralName;
            RatioToSI = ratioToSI;

            Dimension.Units.Add(this);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            Unit other = obj as Unit;
            return other != null
                && Dimension.Equals(other.Dimension)
                && Abbreviation.Equals(other.Abbreviation);
        }

        /// <summary>
        /// Returns the hash code for this <see cref="Unit"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return Dimension.GetHashCode() ^ Abbreviation.GetHashCode();
        }
    }
}
