using System;

namespace PsiFi.Models.Mapping.Items
{
    class Longarm : Weapon
    {
        public override Appearance Appearance { get; } = new Appearance('¬', ConsoleColor.Gray);

        public override Range AttackRange { get; } = new Range(2, 12);

        public override PotentialDamage[] Damage { get; } = new[] {
            new PotentialDamage(1, 8, DamageType.Kinetic),
            new PotentialDamage(1, 8, DamageType.Kinetic),
        };

        public override string Name { get; } = "Longarm";

        public override ItemSlots Slots => ItemSlots.TwoHanded;
    }
}
