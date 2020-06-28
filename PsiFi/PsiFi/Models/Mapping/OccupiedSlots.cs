using System.ComponentModel;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// The slots required to wield an item.
    /// </summary>
    class OccupiedSlots
    {
        /// <summary>
        /// This item must be used in any one hand.
        /// </summary>
        public static readonly OccupiedSlots OffHanded = new OccupiedSlots("off-handed", 1, 0, 0, 0, 0);
     
        /// <summary>
        /// This item must be used in a main hand.
        /// </summary>
        public static readonly OccupiedSlots OneHanded = new OccupiedSlots("one-handed", 1, 1, 0, 0, 0);

        /// <summary>
        /// This item must be used in two hands, one of which is main.
        /// </summary>
        public static readonly OccupiedSlots TwoHanded = new OccupiedSlots("two-handed", 2, 1, 0, 0, 0);

        /// <summary>
        /// This item must be used in three hands, one of which is main.
        /// </summary>
        public static readonly OccupiedSlots Oversized = new OccupiedSlots("over sized", 3, 1, 0, 0, 0);

        /// <summary>
        /// This item is worn on a head.
        /// </summary>
        public static readonly OccupiedSlots Head      = new OccupiedSlots("head"      , 0, 0, 1, 0, 0);

        /// <summary>
        /// This item is worn on a torso.
        /// </summary>
        public static readonly OccupiedSlots Torso     = new OccupiedSlots("torso"     , 0, 0, 0, 1, 0);

        /// <summary>
        /// This item is worn of a pair of legs.
        /// </summary>
        public static readonly OccupiedSlots Legs      = new OccupiedSlots("legs"      , 0, 0, 0, 0, 1);

        /// <summary>
        /// This item is worn on a head, torso and pair of legs.
        /// </summary>
        public static readonly OccupiedSlots FullBody  = new OccupiedSlots("full body" , 0, 0, 1, 1, 1);

        private int[] slots = new int[5];

        /// <summary>
        /// The number of slots required in the specified slot index.
        /// </summary>
        /// <param name="slot">The slot's index.</param>
        public int this[int slot] => slots[slot];

        /// <summary>
        /// The number of slots required of the specified type.
        /// </summary>
        /// <param name="slot">The type of slot.</param>
        public int this[SlotType slot] => slots[(int)slot];

        /// <summary>
        /// A description of this item's size.
        /// </summary>
        public string Description { get; }

        private OccupiedSlots(string description, params int[] slots)
        {
            this.Description = description;
            this.slots = slots;
        }

        public override string ToString() => Description;
    }
}
