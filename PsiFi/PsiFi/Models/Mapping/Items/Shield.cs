using System;

namespace PsiFi.Models.Mapping.Items
{
    class Shield : WieldableItem
    {
        public override Appearance Appearance { get; } = new Appearance('o', ConsoleColor.DarkYellow);

        public override string Name { get; } = "Shield";

        public override OccupiedSlots Slots { get; } = OccupiedSlots.OffHanded;
    }
}
