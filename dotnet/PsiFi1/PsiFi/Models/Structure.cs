namespace PsiFi.Models
{
    /// <summary>
    /// A permanent structure.
    /// </summary>
    abstract class Structure : Occupant
    {
        /// <inheritdoc/>
        public override bool IsStructure => true;

        /// <inheritdoc/>
        public Structure(Appearance appearance) : base(appearance)
        { }
    }
}
