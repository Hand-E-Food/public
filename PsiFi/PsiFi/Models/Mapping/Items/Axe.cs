using System;

namespace PsiFi.Models.Mapping.Items
{
    class Axe:Weapon
    {
        public override Appearance Appearance { get; } = new Appearance('P', ConsoleColor.Gray);

        public override int AttackRange { get; } = 1;

        public override PotentialDamage[] Damage { get; } = new[]
        {
            new PotentialDamage(3, 14, DamageType.Kinetic),
        };

        public override string Name { get; } = "Axe";

        public override ItemSlots Slots { get; } = ItemSlots.TwoHanded;
    }
}
