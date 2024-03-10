using PsiFi.Models;

namespace PsiFi.Rules.Effects
{
    /// <summary>
    /// An effect that applies damage to the target occupant.
    /// </summary>
    class DamageEffect : IEffect
    {
        private readonly DamageRange damageRange;

        /// <summary>
        /// Initialises a new instance of the <see cref="DamageEffect"/> class.
        /// </summary>
        /// <param name="damageRange">The potenatial damage to affect upon the target.</param>
        public DamageEffect(DamageRange damageRange)
        {
            this.damageRange = damageRange;
        }

        /// <summary>
        /// Applies the damage to the target occupant, if any.
        /// </summary>
        /// <inheritdoc/>
        public void Execute(Cell target, State state)
        {
            var occupant = target.Occupant;
            if (occupant == null) return;
            occupant.TakeDamage(state.Random.Next(damageRange));
        }

        public static implicit operator DamageEffect (DamageRange damageRange) => new DamageEffect(damageRange);
    }
}
