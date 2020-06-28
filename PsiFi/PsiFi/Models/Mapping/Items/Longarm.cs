using System;

namespace PsiFi.Models.Mapping.Items
{
    class Longarm : Weapon
    {
        public override Appearance Appearance { get; } = new Appearance('¬', ConsoleColor.Gray);

        public override int AttackRange { get; } = 12;

        public override PotentialDamage[] Damage { get; } = new[] {
            new PotentialDamage(1, 8, DamageType.Kinetic),
            new PotentialDamage(1, 8, DamageType.Kinetic),
        };

        public override string Name { get; } = "Longarm";

        public override OccupiedSlots Slots => OccupiedSlots.TwoHanded;
    }
}
