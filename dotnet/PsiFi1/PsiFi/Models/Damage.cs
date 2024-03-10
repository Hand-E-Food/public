namespace PsiFi.Models
{
    /// <summary>
    /// An absolute amount of damage.
    /// </summary>
    struct Damage
    {
        /// <summary>
        /// The amount of damage.
        /// </summary>
        public int Amount;

        /// <summary>
        /// Initialises a new instance of the <see cref="Damage"/> structure.
        /// </summary>
        /// <param name="amount">The amount of damage.</param>
        public Damage(int amount)
        {
            Amount = amount;
        }
    }
}
