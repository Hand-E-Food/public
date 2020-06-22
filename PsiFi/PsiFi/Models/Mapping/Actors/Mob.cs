using System;
using System.ComponentModel.DataAnnotations;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// An actor with a physical presence.
    /// </summary>
    abstract class Mob : Actor
    {
        /// <summary>
        /// This mob's appearance.
        /// </summary>
        public Appearance Appearance { get; protected set; }

        /// <summary>
        /// The cell this mob is currently in.
        /// </summary>
        public Cell Cell
        {
            get => cell;
            set
            {
                if (cell == value) return;
                if (value?.Mob != null && value.Mob != this) throw new InvalidOperationException("Cell already contains a mob.");

                if (cell != null)
                {
                    var old = cell;
                    cell = null;
                    old.Mob = null;
                }
                cell = value;
                if (cell != null)
                    cell.Mob = this;
            }
        }
        private Cell cell;

        /// <summary>
        /// This mob's health.
        /// </summary>
        public Range Health { get; }

        /// <summary>
        /// Initialises a new instance of a <see cref="Mob"/> class.
        /// </summary>
        /// <param name="appearance">This mob's appearance.</param>
        /// <param name="maxHealth">This mob's maximum health.</param>
        public Mob(Appearance appearance, int maxHealth)
        {
            Appearance = appearance;
            Health = new Range(0, maxHealth);
        }
    }
}
