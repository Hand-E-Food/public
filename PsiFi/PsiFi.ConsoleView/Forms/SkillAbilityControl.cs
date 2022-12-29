using ConsoleForms;
using PsiFi.Interactions;
using System.Diagnostics.CodeAnalysis;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays a protagonist's akill or ability.
    /// </summary>
    internal class SkillAbilityControl : ParentControl
    {
        /// <summary>
        /// Creates a new <see cref="SkillAbilityControl"/>.
        /// </summary>
        /// <param name="ability">The ability to display.</param>
        /// <returns>The new control. May be a subclass.</returns>
        public static SkillAbilityControl Create(Ability ability)
        {
            return new(ability);
        }

        /// <summary>
        /// Creates a new <see cref="SkillAbilityControl"/>.
        /// </summary>
        /// <param name="skill">The skill to display.</param>
        /// <returns>The new control. May be a subclass.</returns>
        public static SkillAbilityControl Create(Skill skill)
        {
            return new(skill);
        }

        public override int? GetDesiredHeight() => 1;

        protected SkillAbilityControl(Ability ability) : this()
        {
            Ability = ability;
            NameControl.SetText(ability.Name);
            Button = CreateButton(ability.GetButtonKey());
        }

        protected SkillAbilityControl(Skill skill) : this()
        {
            Ability = skill.Ability;
            NameControl.SetText(skill.Name, skill.School.GetColor());
            if (Ability != null)
                Button = CreateButton(skill.GetButtonKey());
        }

        private SkillAbilityControl()
        {
            AnchorLeft = 0;
            AnchorTop = 0;
            AnchorRight = 0;

            NameControl = new()
            {
                AnchorLeft = 2,
                AnchorTop = 0,
                AnchorRight = 0,
                AnchorBottom = 0,
            };
            Children.Add(NameControl);
        }

        private Button CreateButton(char key)
        {
            var button = new Button {
                AnchorLeft = 0,
                AnchorTop = 0,
                IsEnabled = false,
                Key = key,
                Action = ButtonAction,
            };
            Children.Insert(0, button);
            return button;
        }

        private void ButtonAction()
        {
            if (interaction == null) return;
            interaction.Response = Ability;
        }

        protected Ability? Ability { get; }

        /// <summary>
        /// The button that selects this ability.
        /// </summary>
        protected Button? Button { get; } = null;

        /// <summary>
        /// The display name control.
        /// </summary>
        protected TextControl NameControl { get; }

        /// <summary>
        /// The current interaction being handled.
        /// </summary>
        public SelectionInteraction<Ability>? Interaction
        {
            protected get => interaction;
            set
            {
                interaction = value;
                if (Button != null)
                    Button.IsEnabled = interaction?.Options.Contains(Ability!) ?? false;
            }
        }
        private SelectionInteraction<Ability>? interaction = null;
    }
}
