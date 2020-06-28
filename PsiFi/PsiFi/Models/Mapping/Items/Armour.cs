using System;
using System.Collections.Generic;
using System.Text;

namespace PsiFi.Models.Mapping.Items
{
    class Armour : WieldableItem
    {
        public override Appearance Appearance { get; } = new Appearance('[', ConsoleColor.Gray);

        public override string Name { get; } = "Armour";

        public override ItemSlots Slots { get; } = ItemSlots.Torso;
    }
}
