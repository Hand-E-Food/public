namespace PsiFi.Models
{
    class Item : IVisible
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
    }
}