namespace PsiFi.Models.Mapping
{
    abstract class SlotsCollection
    {
        /// <summary>
        /// The number of slots.
        /// </summary>
        protected const int SlotCount = 5;

        /// <summary>
        /// The count of each slot.
        /// </summary>
        protected int[] slots;

        /// <summary>
        /// Initialises a new set of wieldable item slots for a <see cref="Mob"/>.
        /// </summary>
        /// <param name="offHands">The number of slots for non-<see cref="SlotType.MainHand"/> hands.</param>
        /// <param name="mainHands">The number of slots for <see cref="SlotType.MainHand"/>.</param>
        /// <param name="heads">The number of slots for <see cref="SlotType.Head"/>.</param>
        /// <param name="torsos">The number of slots for <see cref="SlotType.Torso"/>.</param>
        /// <param name="pairsOfLegs">The number of slots for <see cref="SlotType.Legs"/>.</param>
        protected SlotsCollection(int offHands, int mainHands, int heads, int torsos, int pairsOfLegs)
        {
            slots = new[] { offHands + mainHands, mainHands, heads, torsos, pairsOfLegs };
        }
    }
}
