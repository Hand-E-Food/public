using System;

namespace PsiFi.Models.Mapping.Items
{
    class Handgun : Weapon
    {
        public override Appearance Appearance { get; } = new Appearance('⌐', ConsoleColor.Gray);
     
        public override int AttackRange { get; } = 12;

        public override PotentialDamage[] Damage { get; } = new[] {
            new PotentialDamage(1, 6, DamageType.Kinetic),
            new PotentialDamage(1, 6, DamageType.Kinetic),
        };

        public override string Name { get; } = "Handgun";

        public override OccupiedSlots Slots => OccupiedSlots.OneHanded;
    }
}
