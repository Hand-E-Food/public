namespace PsiFi.Models.Mapping.Items
{
    /// <summary>
    /// A wieldable weapon.
    /// </summary>
    abstract class Weapon : WieldableItem
    {
        /// <summary>
        /// The distance over which this weapon can attack.
        /// </summary>
        public abstract Range AttackRange { get; }

        /// <summary>
        /// This weapon's damage.
        /// </summary>
        public abstract PotentialDamage[] Damage { get; }
    }
}
