using System.Collections.Generic;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// The slots required to wield an item.
    /// </summary>
    class ItemSlots : SlotsCollection
    {
        /// <summary>
        /// This item must be used in any one hand.
        /// </summary>
        public static readonly ItemSlots OffHanded = new ItemSlots("off-handed", 1, 0, 0, 0, 0);
     
        /// <summary>
        /// This item must be used in a main hand.
        /// </summary>
        public static readonly ItemSlots OneHanded = new ItemSlots("one-handed", 0, 1, 0, 0, 0);

        /// <summary>
        /// This item must be used in two hands, one of which is main.
        /// </summary>
        public static readonly ItemSlots TwoHanded = new ItemSlots("two-handed", 1, 1, 0, 0, 0);

        /// <summary>
        /// This item must be used in three hands, one of which is main.
        /// </summary>
        public static readonly ItemSlots Oversized = new ItemSlots("over sized", 2, 1, 0, 0, 0);

        /// <summary>
        /// This item is worn on a head.
        /// </summary>
        public static readonly ItemSlots Head      = new ItemSlots("head"      , 0, 0, 1, 0, 0);

        /// <summary>
        /// This item is worn on a torso.
        /// </summary>
        public static readonly ItemSlots Torso     = new ItemSlots("torso"     , 0, 0, 0, 1, 0);

        /// <summary>
        /// This item is worn of a pair of legs.
        /// </summary>
        public static readonly ItemSlots Legs      = new ItemSlots("legs"      , 0, 0, 0, 0, 1);

        /// <summary>
        /// This item is worn on a head, torso and pair of legs.
        /// </summary>
        public static readonly ItemSlots FullBody  = new ItemSlots("full body" , 0, 0, 1, 1, 1);

        /// <summary>
        /// A description of this item's size.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// This item's slots. For internal use only.
        /// </summary>
        public int this[int slot] => slots[slot];

        /// <summary>
        /// Initialises a new set of wieldable item slots for a <see cref="Mob"/>.
        /// </summary>
        /// <param name="offHands">The required number of slots for non-<see cref="SlotType.MainHand"/> hands.</param>
        /// <param name="mainHands">The required number of slots for <see cref="SlotType.MainHand"/>.</param>
        /// <param name="heads">The required number of slots for <see cref="SlotType.Head"/>.</param>
        /// <param name="torsos">The required number of slots for <see cref="SlotType.Torso"/>.</param>
        /// <param name="pairsOfLegs">The required number of slots for <see cref="SlotType.Legs"/>.</param>
        private ItemSlots(string description, int offHands, int mainHands, int heads, int torsos, int pairsOfLegs)
            : base(offHands, mainHands, heads, torsos, pairsOfLegs)
        {
            Description = description;
        }

        public override string ToString() => Description;
    }
}
