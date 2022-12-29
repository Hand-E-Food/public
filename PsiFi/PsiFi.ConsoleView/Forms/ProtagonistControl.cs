using ConsoleForms;
using PsiFi.Interactions;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays a protagonist.
    /// </summary>
    internal class ProtagonistControl : ParentControl
    {
        private readonly TextControl nameControl;
        private readonly RangeControl healthControl;
        private readonly SkillAbilityListControl skillAbilitiesControl;

        /// <summary>
        /// Creates a new <see cref="ProtagonistControl"/>.
        /// </summary>
        public ProtagonistControl()
        {
            nameControl = new() {
                AnchorLeft = 0,
                AnchorRight = 0,
                AnchorTop = 0,
                MaximumHeight = 1
            };
            Children.Add(nameControl);

            healthControl = new() {
                AnchorLeft = 0,
                AnchorRight = 0,
                AnchorTop = 1,
                ForegroundColor = ConsoleColor.Red,
                Formats = new[] {
                    RangeFormat.Pips('♥'),
                    RangeFormat.Fraction,
                    RangeFormat.Percentage,
                    RangeFormat.Value,
                    RangeFormat.Overflow("↑↑↑"),
                }
            };
            Children.Add(healthControl);

            skillAbilitiesControl = new() {
                AnchorLeft = 0,
                AnchorRight = 0,
                AnchorTop = 2,
                AnchorBottom = 0
            };
            Children.Add(skillAbilitiesControl);
        }

        /// <summary>
        /// The protagonist to display.
        /// </summary>
        public Protagonist? Protagonist 
        {
            get => protagonist;
            set
            {
                if (protagonist == value) return;
                if (protagonist != null) protagonist.PropertyChanged -= Protagonist_PropertyChanged;
                protagonist = value;
                if (protagonist != null)
                {
                    protagonist.PropertyChanged += Protagonist_PropertyChanged;
                    nameControl.SetText(protagonist.Name);
                    healthControl.Range = protagonist.Health;
                    skillAbilitiesControl.SetAbilitiesFrom(protagonist);
                }
                else
                {
                    nameControl.SetText();
                    healthControl.Range = null;
                    skillAbilitiesControl.ClearAbilities();
                }
                InvalidateDrawing();
            }
        }
        private Protagonist? protagonist = null;

        private void Protagonist_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Protagonist.Name):
                    nameControl.SetText(Protagonist!.Name);
                    break;

                case nameof(Protagonist.Abilities):
                case nameof(Protagonist.Skills):
                    skillAbilitiesControl.SetAbilitiesFrom(Protagonist!);
                    break;
            }
        }

        /// <summary>
        /// The current ability selection interaction.
        /// </summary>
        public SelectionInteraction<Ability>? Interaction
        {
            get => skillAbilitiesControl.Interaction;
            set => skillAbilitiesControl.Interaction = value;
        }
    }
}