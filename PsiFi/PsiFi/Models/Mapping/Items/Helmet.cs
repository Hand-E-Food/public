﻿using System;

namespace PsiFi.Models.Mapping.Items
{
    class Helmet : WieldableItem
    {
        public override Appearance Appearance { get; } = new Appearance('∩', ConsoleColor.Gray);

        public override string Name { get; } = "Helmet";

        public override ItemSlots Slots { get; } = ItemSlots.Head;
    }
}