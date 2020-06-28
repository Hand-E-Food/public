namespace PsiFi.Models.Mapping
{
    class PotentialDamage
    {
        /// <summary>
        /// The possible range of damage applied.
        /// </summary>
        public Range Range { get; }

        /// <summary>
        /// The type of damage applied.
        /// </summary>
        public DamageType Type { get; }

        /// <summary>
        /// Initialises a new damage potential.
        /// </summary>
        /// <param name="amount">The amount of damage applied.</param>
        /// <param name="type">The type of damage applied.</param>
        public PotentialDamage(int amount, DamageType type)
        {
            Range = new Range(amount, amount);
            Type = type;
        }

        /// <summary>
        /// Initialises a new damage potential.
        /// </summary>
        /// <param name="minimum">The minimum damage applied.</param>
        /// <param name="maximum">The maximum damage applied.</param>
        /// <param name="type">The type of damage applied.</param>
        public PotentialDamage(int minimum, int maximum, DamageType type)
        {
            Range = new Range(minimum, maximum);
            Type = type;
        }

        /// <summary>
        /// Initialises a new damage potential.
        /// </summary>
        /// <param name="range">The possible range of damage applied.</param>
        /// <param name="type">The type of damage applied.</param>
        public PotentialDamage(Range range, DamageType type)
        {
            Range = range;
            Type = type;
        }
    }
}
