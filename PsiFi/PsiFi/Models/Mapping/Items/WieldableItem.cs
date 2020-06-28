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
                    mob.Slots.Release(Slots);

                mob = value;

                if (mob != null)
                    mob.Slots.Consume(Slots);
            }
        }
        private Mob mob = null;

        /// <summary>
        /// The slots consumed by this item.
        /// </summary>
        public abstract OccupiedSlots Slots { get; }
    }
}
