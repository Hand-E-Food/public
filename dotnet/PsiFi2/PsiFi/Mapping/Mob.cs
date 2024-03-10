using PsiFi.Mapping.Actions;
using Rogue;
using System.Collections.Generic;
using static System.Math;

namespace PsiFi.Mapping
{
    /// <summary>
    /// An actor physically present on the map.
    /// </summary>
    public abstract class Mob : IActor, IPhysical
    {
        /// <summary>
        /// This mob's attributes.
        /// </summary>
        public ICollection<string> Attributes { get; } = new SortedSet<string>();

        /// <summary>
        /// This mob's cell.
        /// </summary>
        public Cell? Cell
        {
            get => cell;
            set
            {
                if (cell == value) return;
                if (cell != null) cell.Mob = null;
                cell = value;
                if (cell != null) cell.Mob = this;
            }
        }
        private Cell? cell = null;

        /// <inheritdoc/>
        public abstract char Character { get; }

        /// <summary>
        /// This mob's currently equipped weapon.
        /// </summary>
        public Item? EquippedWeapon { get; set; } = null;

        /// <inheritdoc/>
        public abstract Color ForeColor { get; }

        /// <summary>
        /// This mob's current health.
        /// </summary>
        public virtual int Health
        {
            get => health;
            set => health = Min(value, MaximumHealth);
        }
        private int health;

        /// <summary>
        /// This breed's maximum health.
        /// </summary>
        public abstract int MaximumHealth { get; }

        /// <summary>
        /// This breed's name.
        /// </summary>
        public abstract string Name { get; }

        /// <inheritdoc/>
        public int TimeUntilNextTurn { get; protected set; } = 0;

        /// <summary>
        /// Initialises a new instance of the <see cref="Mob"/> class.
        /// </summary>
        public Mob()
        {
            Health = MaximumHealth;
        }

        /// <inheritdoc/>
        public abstract IAction Act();

        /// <inheritdoc/>
        public virtual void PassTime(int milliseconds) => TimeUntilNextTurn -= milliseconds;
    }
}
