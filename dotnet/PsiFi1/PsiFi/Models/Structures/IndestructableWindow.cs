namespace PsiFi.Models.Structures
{
    /// <summary>
    /// A window that can never be destroyed.
    /// </summary>
    class IndestructableWindow : Window
    {
        /// <summary>
        /// Ignores all damage.
        /// </summary>
        /// <param name="damage">The damage to ignore.</param>
        public override void TakeDamage(Damage damage)
        { }

        /// <summary>
        /// Initialises an <see cref="IndestructableWindow"/> with the default appearance.
        /// </summary>
        public IndestructableWindow() : base()
        { }

        /// <summary>
        /// Initialises an <see cref="IndestructableWindow"/>.
        /// </summary>
        /// <param name="appearance">This window's appearance.</param>
        public IndestructableWindow(Appearance appearance) : base(appearance)
        { }
    }
}