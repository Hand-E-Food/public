using System.Diagnostics;

namespace PsiFi
{
    /// <summary>
    /// Damage to be suffered.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public struct Damage
    {
        internal string DebuggerDisplay => $"{Amount} damage";

        /// <summary>
        /// Initialises a new <see cref="Damage"/>.
        /// </summary>
        /// <param name="amount">The amount of damage to suffer.</param>
        public Damage(int amount)
        {
            Amount = amount;
        }

        /// <summary>
        /// The amount of damage to suffer.
        /// </summary>
        public int Amount;
    }
}
