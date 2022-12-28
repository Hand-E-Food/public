using PsiFi.Abilities;
using PsiFi.Skills;

namespace PsiFi
{
    /// <summary>
    /// A protagonist.
    /// </summary>
    public class Protagonist : Mob
    {
        /// <summary>
        /// Creates a new <see cref="Protagonist"/>.
        /// </summary>
        public Protagonist() : base(10)
        {
            Abilities = new[]
            {
                new TargetAbility(this),
            };
            WieldedWeapon = new Sword(this);
            Skills = new(1 + 6 * 2 + 4)
            {
                WieldedWeapon,
            };
        }

        public override string Name => "Protagonist";

        /// <summary>
        /// This protagonist's skills.
        /// </summary>
        public List<Skill> Skills { get; }

        /// <summary>
        /// This protagonist's currently wielded weapon.
        /// </summary>
        public Weapon WieldedWeapon { get; set; }
    }
}
