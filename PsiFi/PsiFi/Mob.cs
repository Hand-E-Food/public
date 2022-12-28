using System.Diagnostics;

namespace PsiFi
{
    /// <summary>
    /// A mob.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public abstract class Mob
    {
        internal string DebuggerDisplay => $"{Name}, health: {Health.DebuggerDisplay}";

        /// <summary>
        /// Creates a new <see cref="Mob"/>.
        /// </summary>
        /// <param name="health">This mob's maximum and initial health.</param>
        public Mob(int health)
        {
            Health = new Range(health);
        }

        /// <summary>
        /// This mob's natural abilities.
        /// </summary>
        public ICollection<Ability> Abilities { get; protected set; } = Array.Empty<Ability>();

        /// <summary>
        /// The target of this mob's attacks.
        /// </summary>
        public Mob? AttackTarget { get; set; }

        /// <summary>
        /// This mob's health.
        /// </summary>
        public Range Health { get; }

        /// <summary>
        /// This mob's name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The room this mob is in, if any.
        /// </summary>
        public Room? Room {
            get => room;
            set
            {
                if (room != null) room.Mobs.Remove(this);
                room = value;
                if (room != null) room.Mobs.Add(this);
            }
        }
        private Room? room = null;

        /// <summary>
        /// Causes this mob to die.
        /// </summary>
        public void Die()
        {
            Room = null;
        }

        /// <summary>
        /// Causes this mob to suffer damage.
        /// </summary>
        /// <param name="damage">The damage to suffer.</param>
        /// <returns>The damage actually suffered.</returns>
        public Damage Suffer(Damage damage)
        {
            Health.Value -= damage.Amount;
            if (Health <= 0) Die();
            return damage;
        }
    }
}
