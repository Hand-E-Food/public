using System;
using System.Runtime.InteropServices;

namespace PsiFi.Models.Mapping.Items
{
    /// <summary>
    /// An item that can be worn.
    /// </summary>
    abstract class WieldableItem : Item
    {
        /// <summary>
        /// The mob holding this item.
        /// </summary>
        public Mob Mob 
        {
            get => mob;
            set
            {
                if (mob == value) return;

                if (mob != null)
                    mob.Slots.TryRemove(this);

                if (value != null && !value.Slots.TryAdd(this))
                    throw new InvalidOperationException($"{mob.Name} cannot currently wield {Name}.");
                
                mob = value;
            }
        }
        private Mob mob = null;

        /// <summary>
        /// The slots consumed by this item.
        /// </summary>
        public abstract ItemSlots Slots { get; }
    }
}
