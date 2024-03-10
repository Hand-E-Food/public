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
        public Protagonist() : base(health: 10)
        {
            Abilities = new[]
            {
                new TargetAbility(this),
            };
            var defaultWeapon = new Sword(this);
            LearnSkill(defaultWeapon);
            wieldedWeapon = defaultWeapon;
        }

        public override string Name => "Protagonist";

        /// <summary>
        /// Adds the specified skill to this prtagonist's skills.
        /// </summary>
        /// <param name="skill">The skill to add.</param>
        public void LearnSkill(Skill skill)
        {
            skills.Add(skill);
            OnPropertyChanged(nameof(Skills));
        }

        /// <summary>
        /// This protagonist's skills.
        /// </summary>
        public IReadOnlyCollection<Skill> Skills => skills;
        private readonly List<Skill> skills = new(1 + 6 * 2 + 4);

        /// <summary>
        /// This protagonist's currently wielded weapon.
        /// </summary>
        public Weapon WieldedWeapon
        {
            get => wieldedWeapon;
            set
            {
                if (wieldedWeapon == value) return;
                wieldedWeapon = value;
                OnPropertyChanged();
            }
        }
        private Weapon wieldedWeapon;
    }
}
