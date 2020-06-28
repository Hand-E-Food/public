using System;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// A specific amount of damage.
    /// </summary>
    /// <remarks>
    /// Used for received damage and damage resistance.
    /// </remarks>
    struct Damage
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

        public static Damage operator +(Damage damage, int amount) => new Damage(Math.Max(0, damage.Amount + amount), damage.Type);

        public static Damage operator -(Damage damage, int amount) => new Damage(Math.Max(0, damage.Amount - amount), damage.Type);

        /// <summary>
        /// Applies damage resistance to received damage.
        /// </summary>
        /// <param name="damage">The damage received.</param>
        /// <param name="resistance">The damage resistance.</param>
        /// <returns>
        /// If the received damage and damage resistance are of the same type, returns a new damage amount equal to their difference (minimum zero.)
        /// If the received damage and damage resistance are of different types, returns <paramref name="damage"/> unchnaged.
        /// </returns>
        public static Damage operator -(Damage damage, Damage resistance)
        {
            return damage.Type == resistance.Type
                ? damage - resistance.Amount
                : damage;
        }
    }
}
