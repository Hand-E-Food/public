using System;

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
        public abstract Appearance Appearance { get; }

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
        public abstract Range Health { get; }
    }
}
