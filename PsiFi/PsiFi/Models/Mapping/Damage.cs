namespace PsiFi.Models.Mapping
{
    class Damage
    {
        /// <summary>
        /// The possible range of damage applied.
        /// </summary>
        public int Amount { get; }

        /// <summary>
        /// The type of damage applied.
        /// </summary>
        public DamageType Type { get; }

        /// <summary>
        /// Initialises a new damage.
        /// </summary>
        /// <param name="amount">The amount of damage applied.</param>
        /// <param name="type">The type of damage applied.</param>
        public Damage(int amount, DamageType type)
        {
            Amount = amount;
            Type = type;
        }
    }
}
