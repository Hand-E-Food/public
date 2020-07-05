using System;

namespace PsiFi.Models.Mapping.Items
{
    class Handgun : Weapon
    {
        public override Appearance Appearance { get; } = new Appearance('⌐', ConsoleColor.Gray);

        public override Range AttackRange { get; } = new Range(1, 9);

        public override PotentialDamage[] Damage { get; } = new[] {
            new PotentialDamage(1, 6, DamageType.Kinetic),
            new PotentialDamage(1, 6, DamageType.Kinetic),
        };

        public override string Name { get; } = "Handgun";

        public override ItemSlots Slots => ItemSlots.OneHanded;
    }
}
