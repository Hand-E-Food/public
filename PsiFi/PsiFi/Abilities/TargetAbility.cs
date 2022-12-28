using PsiFi.Interactions;

namespace PsiFi.Abilities
{
    /// <summary>
    /// The ability to select a target for future attacks.
    /// </summary>
    public class TargetAbility : Ability
    {
        /// <summary>
        /// Creates a new <see cref="TargetAbility"/>.
        /// </summary>
        /// <param name="protagonist">The protagonist that owns this ability.</param>
        public TargetAbility(Protagonist protagonist)
        {
            Protagonist = protagonist;
        }

        /// <summary>
        /// The protagonist that owns this ability.
        /// </summary>
        protected Protagonist Protagonist { get; }

        public bool CanActivateAbility => Protagonist.Room?.OtherMobs.Any() ?? false;
        public string Name => "Select target";

        public IEnumerable<Interaction> ActivateAbility()
        {
            if (Protagonist.Room == null) throw new InvalidOperationException("The protagonist must be in a room.");
            var otherMobs = Protagonist.Room.OtherMobs.ToList();
            switch (otherMobs.Count)
            {
                case 0:
                    Protagonist.AttackTarget = null;
                    break;
                case 1:
                    Protagonist.AttackTarget = otherMobs[0];
                    break;
                default:
                    SelectionInteraction<Mob> selection = new(otherMobs, "Select target");
                    yield return selection;
                    Protagonist.AttackTarget = selection.Response;
                    break;
            }
        }
    }
}
