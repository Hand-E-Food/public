namespace PsiFi.Models
{
    abstract class Occupant : IVisible
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
        /// True if this occupant completely blocks energy such as light or laser beams.
        /// False if energy can pass by this occupant.
        /// </summary>
        public abstract bool BlocksEnergy { get; }

        /// <summary>
        /// True if this occupant compeltely blocks physical projectiles such as bullets.
        /// False if projectiles can pass by this occupant.
        /// </summary>
        public abstract bool BlocksProjectiles { get; }

        /// <summary>
        /// This occupant's health.
        /// </summary>
        public virtual int Health { get; set; }

        /// <summary>
        /// True if this occupant is a non-moving structure.
        /// False if this occupant moves.
        /// </summary>
        public abstract bool IsStructure { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Occupant"/> class.
        /// </summary>
        /// <param name="appearance">The default appearance.</param>
        public Occupant(Appearance appearance)
        {
            Appearance = appearance;
        }

        /// <summary>
        /// Reduces this occupant's health.
        /// </summary>
        /// <param name="damage">The damage to apply.</param>
        public virtual void TakeDamage(Damage damage)
        {
            Health -= damage.Amount;
        }
    }
}