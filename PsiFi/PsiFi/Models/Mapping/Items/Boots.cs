using System;

namespace PsiFi.Models.Mapping.Items
{
    class Boots : WieldableItem
    {
        public override Appearance Appearance { get; } = new Appearance('«', ConsoleColor.Gray);

        public override string Name { get; } = "Boots";

        public override OccupiedSlots Slots { get; } = OccupiedSlots.Legs;
    }
}
