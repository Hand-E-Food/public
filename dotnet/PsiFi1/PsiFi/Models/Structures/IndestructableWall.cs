namespace PsiFi.Models.Structures
{
    /// <summary>
    /// A wall that can never be destroyed.
    /// </summary>
    class IndestructableWall : Wall
    {
        /// <summary>
        /// Ignores all damage.
        /// </summary>
        /// <param name="damage">The damage to ignore.</param>
        public override void TakeDamage(Damage damage)
        { }

        /// <summary>
        /// Initialises an <see cref="IndestructableWall"/> with the default appearance.
        /// </summary>
        public IndestructableWall() : base()
        { }

        /// <summary>
        /// Initialises an <see cref="IndestructableWall"/>.
        /// </summary>
        /// <param name="appearance">This wall's appearance.</param>
        public IndestructableWall(Appearance appearance) : base(appearance)
        { }
    }
}
