using PsiFi.Geometry;

namespace PsiFi.Models
{
    /// <summary>
    /// A potential range of damage.
    /// </summary>
    class DamageRange
    {
        /// <summary>
        /// The dice detailing the potential range of damage that can be applied.
        /// </summary>
        public Dice Dice { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DamageRange"/> class.
        /// </summary>
        /// <param name="dice">The collection of dice to roll for this damage.</param>
        public DamageRange(string dice)
        {
            Dice = new Dice(dice);
        }
    }
}
