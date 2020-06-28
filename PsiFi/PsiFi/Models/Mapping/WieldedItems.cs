using PsiFi.Models.Mapping.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// The slots available to a mob for wielding items.
    /// </summary>
    class WieldedItems : SlotsCollection
    {
        /// <summary>
        /// Creates an <see cref="WieldedItems"/> object with no slots.
        /// </summary>
        public static WieldedItems None() => new WieldedItems(0, 0, 0, 0, 0);

        /// <summary>
        /// Creates an <see cref="WieldedItems"/> object with two hands.
        /// </summary>
        public static WieldedItems Hands() => new WieldedItems(1, 1, 0, 0, 0);

        /// <summary>
        /// Creates an <see cref="WieldedItems"/> object with two hands, one head, one toros and a pair of legs.
        /// </summary>
        public static WieldedItems Humanoid() => new WieldedItems(1, 1, 1, 1, 1);

        /// <summary>
        /// The items in this collection.
        /// </summary>
        public IReadOnlyList<WieldableItem> Items { get; }
        private readonly List<WieldableItem> items = new List<WieldableItem>();

        /// <summary>
        /// Initialises a new set of wieldable item slots for a <see cref="Mob"/>.
        /// </summary>
        /// <param name="offHands">The number of slots for non-<see cref="SlotType.MainHand"/> hands.</param>
        /// <param name="mainHands">The number of slots for <see cref="SlotType.MainHand"/>.</param>
        /// <param name="heads">The number of slots for <see cref="SlotType.Head"/>.</param>
        /// <param name="torsos">The number of slots for <see cref="SlotType.Torso"/>.</param>
        /// <param name="pairsOfLegs">The number of slots for <see cref="SlotType.Legs"/>.</param>
        public WieldedItems(int offHands, int mainHands, int heads, int torsos, int pairsOfLegs)
            : base(offHands, mainHands, heads, torsos, pairsOfLegs)
        {
            Items = new ReadOnlyCollection<WieldableItem>(items);
        }

        /// <summary>
        /// Checks whether there are enough slots available to wield the item.
        /// </summary>
        /// <param name="item">The item to wield.</param>
        /// <returns>
        /// True if there are enough slots available.
        /// False if there are not enough slots available.
        /// </returns>
        public bool CanAdd(WieldableItem item)
        {
            for (int i = 0; i < SlotCount; i++)
                if (slots[i] < item.Slots[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Checks whether the item can be removed from this collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>
        /// True if the item can be removed.
        /// False if the item is not in this collection.
        /// </returns>
        public bool CanRemove(WieldableItem item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Wields the item and consumes available slots.
        /// </summary>
        /// <param name="item">The item to wield.</param>
        /// <returns>
        /// True if there were enough slots available.
        /// False if there are not enough slots available.
        /// </returns>
        public bool TryAdd(WieldableItem item)
        {
            if (!CanAdd(item)) return false;

            for (int i = 0; i < SlotCount; i++)
                slots[i] -= item.Slots[i];

            items.Add(item);
            return true;
        }

        /// <summary>
        /// Removes the item and releases consumed slots.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>
        /// True if the item was removed.
        /// False if the item is not in this collection.
        /// </returns>
        public bool TryRemove(WieldableItem item)
        {
            if (!CanRemove(item))
                return false;

            for (int i = 0; i < SlotCount; i++)
                slots[i] += item.Slots[i];

            return true;
        }
    }
}
