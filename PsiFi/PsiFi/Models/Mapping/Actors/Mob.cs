using PsiFi.Models.Mapping.Items;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
        /// Called when <see cref="Health"/> is zero. Removes this mob from the map and drops all of its inventory.
        /// </summary>
        public void Die()
        {
            var cell = Cell;
            Cell = null;
            foreach (var item in Inventory)
            {
                if (cell.Item == null)
                    cell.Item = item;
            }
            Inventory?.Clear();
        }

        /// <summary>
        /// This mob's health.
        /// </summary>
        public abstract RangeValue Health { get; }

        /// <summary>
        /// The items this mob is carrying.
        /// </summary>
        public abstract List<Item> Inventory { get; }

        /// <summary>
        /// This mob's available item slots.
        /// </summary>
        public abstract WieldedItems Slots { get; }
    }
}
