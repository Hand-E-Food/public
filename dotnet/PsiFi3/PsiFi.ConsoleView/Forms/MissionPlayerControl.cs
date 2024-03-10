using ConsoleForms;
using PsiFi.Interactions;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays the protagonist, and mission-level abilities.
    /// </summary>
    internal class MissionPlayerControl : ParentControl
    {
        /// <summary>
        /// The control that displays the protagonist.
        /// </summary>
        public ProtagonistControl ProtagonistControl { get; }

        /// <summary>
        /// The control that displays mission-level abilities.
        /// </summary>
        public SkillAbilityListControl MissionAbilitiesControl { get; }

        /// <summary>
        /// Creates a new <see cref="MissionPlayerControl"/>.
        /// </summary>
        public MissionPlayerControl()
        {
            SetDesiredWidth(20);

            ProtagonistControl = new() {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 0,
            };
            Children.Add(ProtagonistControl);

            MissionAbilitiesControl = new() {
                AnchorLeft = 0,
                AnchorRight = 0,
                AnchorBottom = 0,
            };
            Children.Add(MissionAbilitiesControl);
        }

        /// <summary>
        /// The protagonist to display.
        /// </summary>
        public Protagonist? Protagonist
        {
            get => ProtagonistControl.Protagonist;
            set => ProtagonistControl.Protagonist = value;
        }

        /// <summary>
        /// Sets the mission-level abilities to display.
        /// </summary>
        /// <param name="abilities">The abilities to display.</param>
        public void SetMissionAbilities(IEnumerable<Ability> abilities)
        {
            MissionAbilitiesControl.SetAbilitiesFrom(abilities);
            InvalidateLayout();
        }

        public void Interact(SelectionInteraction<Ability> interaction)
        {
            ProtagonistControl.Interaction = interaction;
            MissionAbilitiesControl.Interaction = interaction;
            Canvas!.ReadUserInput();
            ProtagonistControl.Interaction = null;
            MissionAbilitiesControl.Interaction = null;
        }
    }
}
