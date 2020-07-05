using System;

namespace PsiFi.Models.Mapping.Items
{
    class Sword : Weapon
    {
        public override Appearance Appearance { get; } = new Appearance('|', ConsoleColor.Gray);

        public override Range AttackRange { get; } = new Range(1, 1);

        public override PotentialDamage[] Damage { get; } = new[]
        {
            new PotentialDamage(2, 11, DamageType.Kinetic),
        };

        public override string Name { get; } = "Sword";

        public override ItemSlots Slots { get; } = ItemSlots.OneHanded;
    }
}
