namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// The slots available to a mob for wielding items.
    /// </summary>
    class AvailableSlots
    {
        private const int SlotCount = 5;

        /// <summary>
        /// Creates an <see cref="AvailableSlots"/> object with no slots.
        /// </summary>
        public static AvailableSlots None() => new AvailableSlots(0, 0, 0, 0, 0);

        /// <summary>
        /// Creates an <see cref="AvailableSlots"/> object with two hands.
        /// </summary>
        public static AvailableSlots Hands() => new AvailableSlots(2, 1, 0, 0, 0);

        /// <summary>
        /// Creates an <see cref="AvailableSlots"/> object with two hands, one head, one toros and a pair of legs.
        /// </summary>
        public static AvailableSlots Humanoid() => new AvailableSlots(2, 1, 1, 1, 1);

        private int[] slots;

        /// <summary>
        /// Gets the number of slots available at the specified slot index.
        /// </summary>
        /// <param name="slot">The slot's index.</param>
        public int this[int slot] => slots[slot];

        /// <summary>
        /// Gets the number of slots available of the specified type.
        /// </summary>
        /// <param name="slot">The type of slot.</param>
        public int this[SlotType slot] => slots[(int)slot];

        /// <summary>
        /// Initialises a new set of wieldable item slots for a <see cref="Mob"/>.
        /// </summary>
        /// <param name="hands">The number of slots for <see cref="SlotType.Hand"/>.</param>
        /// <param name="mainHands">The number of slots for <see cref="SlotType.MainHand"/>.</param>
        /// <param name="heads">The number of slots for <see cref="SlotType.Head"/>.</param>
        /// <param name="torsos">The number of slots for <see cref="SlotType.Torso"/>.</param>
        /// <param name="pairsOfLegs">The number of slots for <see cref="SlotType.Legs"/>.</param>
        public AvailableSlots(int hands, int mainHands, int heads, int torsos, int pairsOfLegs)
        {
            slots = new[] { hands, mainHands, heads, torsos, pairsOfLegs };
        }

        /// <summary>
        /// Checks whether there are enough slots available to consume.
        /// </summary>
        /// <param name="occupiedSlots">The slots to consume.</param>
        /// <returns>
        /// True if there are enough slots available.
        /// False if there are not enough slots available.
        /// </returns>
        public bool CanConsume(OccupiedSlots occupiedSlots)
        {
            for (int i = 0; i < SlotCount; i++)
                if (slots[i] < occupiedSlots[i])
                    return false;
            return true;
        }

        /// <summary>
        /// Consumes available slots.
        /// </summary>
        /// <param name="occupiedSlots">The slots to consume.</param>
        public void Consume(OccupiedSlots occupiedSlots)
        {
            for (int i = 0; i < SlotCount; i++)
                slots[i] -= occupiedSlots[i];
        }

        /// <summary>
        /// Releases consumed slots.
        /// </summary>
        /// <param name="occupiedSlots">The slots to release.</param>
        public void Release(OccupiedSlots occupiedSlots)
        {
            for (int i = 0; i < SlotCount; i++)
                slots[i] += occupiedSlots[i];
        }
    }
}
