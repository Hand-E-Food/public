namespace PsiFi.Models
{
    /// <summary>
    /// A floor tile.
    /// </summary>
    class Floor : IVisible
    {
        /// <inheritdoc/>
        public Appearance Appearance
        {
            get => appearance;
            set
            {
                appearance = value;
                AppearanceChanged?.Invoke();
            }
        }
        private Appearance appearance;
        /// <inheritdoc/>
        public event SimpleEventHandler? AppearanceChanged;

        /// <summary>
        /// Initialises a new instance of the <see cref="Floor"/> class.
        /// </summary>
        /// <param name="appearance">The default appearance.</param>
        public Floor(Appearance appearance)
        {
            Appearance = appearance;
        }
    }
}