using ConsoleForms;
using PsiFi.Interactions;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays a list of abilities and skills.
    /// </summary>
    internal class SkillAbilityListControl : ScrollList
    {
        public override int? GetDesiredHeight() => Children.Sum(child => child.GetDesiredHeight());

        public void ClearAbilities() => Children.Clear();

        public void SetAbilitiesFrom(Protagonist protagonist)
        {
            SetChildren(Enumerable.Concat(
                protagonist.Abilities.Select(SkillAbilityControl.Create),
                protagonist.Skills.Select(SkillAbilityControl.Create)
            ));
        }

        public void SetAbilitiesFrom(Mob mob) =>
            SetChildren(mob.Abilities.Select(SkillAbilityControl.Create));

        public void SetAbilitiesFrom(IEnumerable<Ability> abilities) =>
            SetChildren(abilities.Select(SkillAbilityControl.Create));

        private void SetChildren(IEnumerable<SkillAbilityControl> children)
        {
            Children.Clear();
            foreach(var child in children)
            {
                child.Interaction = interaction;
                Children.Add(child);
            }
        }

        /// <summary>
        /// The current ability selection interaction.
        /// </summary>
        public SelectionInteraction<Ability>? Interaction
        {
            get => interaction;
            set
            {
                interaction = value;
                foreach (var child in Children.OfType<SkillAbilityControl>())
                    child.Interaction = value;
            }
        }
        private SelectionInteraction<Ability>? interaction;
    }
}
