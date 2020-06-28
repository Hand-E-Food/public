namespace PsiFi.Models.Mapping
{
    enum SlotType
    {
        /// <summary>
        /// A hand slot.
        /// </summary>
        Hand = 0,

        /// <summary>
        /// A main hand slot. A <see cref="Hand"/> slot should also be provided.
        /// </summary>
        MainHand = 1,

        /// <summary>
        /// A head slot.
        /// </summary>
        Head = 2,

        /// <summary>
        /// A torso slot.
        /// </summary>
        Torso = 3,

        /// <summary>
        /// A pair of legs slot.
        /// </summary>
        Legs = 4,
    }

    static class SlotTypes
    {
        public static readonly SlotType[] All =
        {
            SlotType.Hand,
            SlotType.MainHand,
            SlotType.Head,
            SlotType.Torso,
            SlotType.Legs,
        };
    }
}