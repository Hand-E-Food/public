using PsiFi.Interactions;
using System.Text;

namespace PsiFi.Abilities
{
    /// <summary>
    /// The ability to attack mobs.
    /// </summary>
    public class AttackAbility : Ability
    {
        /// <summary>
        /// Initialises a new <see cref="AttackAbility"/>.
        /// </summary>
        /// <param name="mob">The mob that has this ability.</param>
        /// <param name="damage">The damage done by this attack.</param>
        public AttackAbility(Mob mob, string name, Damage damage)
        { 
            Damage = damage;
            Mob = mob;
            Name = name;
        }

        /// <summary>
        /// The damage done by this attack.
        /// </summary>
        public Damage Damage { get; }

        /// <summary>
        /// The mob that has this ability.
        /// </summary>
        protected Mob Mob { get; }

        /// <summary>
        /// This attack's name.
        /// </summary>
        public string Name { get; }

        public bool CanActivateAbility => Mob.AttackTarget != null;

        public IEnumerable<Interaction> ActivateAbility()
        {
            if (Mob.AttackTarget == null) throw new InvalidOperationException("Cannot attack without a target.");
            var target = Mob.AttackTarget;
            var damage = target.Suffer(Damage);
            var message = new StringBuilder();
            message.Append(Mob.Name);
            message.Append(" attacks ");
            message.Append(target.Name);
            message.Append(" with ");
            message.Append(Name);
            message.Append(" doing ");
            message.Append(damage.Amount);
            message.Append(" damage");
            if (target.Health <= 0) message.Append(" and dies");
            message.Append('.');
            yield return new NotificationInteraction(message.ToString());
        }
    }
}
