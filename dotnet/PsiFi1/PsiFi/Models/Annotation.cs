namespace PsiFi.Models
{
    class Annotation : IVisible
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
        /// Initialises a new instance of the <see cref="Annotation"/> class.
        /// </summary>
        /// <param name="appearance">The default appearance.</param>
        public Annotation(Appearance appearance)
        {
            Appearance = appearance;
        }
    }
}