using ConsoleForms;
using PsiFi.Interactions;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays log messages.
    /// </summary>
    internal class LogControl : ScrollList
    {
        /// <summary>
        /// This control's desired height.
        /// </summary>
        public int? DesiredHeight
        {
            get => GetDesiredHeight();
            set => SetDesiredHeight(value);
        }

        public void Interact(NotificationInteraction interaction)
        {
            bool shouldScroll = ScrollOffset == ScrollBottom;
            TextControl textControl = new() {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 0,
            };
            textControl.SetText(interaction.Message);
            Children.Add(textControl);
            if (shouldScroll) ScrollOffset = ScrollBottom;
        }

        public void Interact(OkInteraction interaction)
        {
            var logInteractionControl = new LogInteractionControl(interaction.Message);
            logInteractionControl.AddButton(Key.Enter, Button.NoAction);

            Children.Add(logInteractionControl);
            ScrollOffset = ScrollBottom;

            Canvas!.ReadUserInput();

            Children.Replace(logInteractionControl, logInteractionControl.RemoveInteraction());
            ScrollOffset = Math.Max(ScrollOffset, ScrollBottom);
        }

        public void Interact(YesNoInteraction interaction)
        {
            var logInteractionControl = new LogInteractionControl(interaction.Message);
            logInteractionControl.AddButton('y', () => interaction.Response = true);
            logInteractionControl.AddButton('n', () => interaction.Response = false);
            Children.Add(logInteractionControl);

            ScrollOffset = ScrollBottom;
            Canvas!.ReadUserInput();

            var answer = interaction.Response ? " yes" : " no";
            Children.Replace(logInteractionControl, logInteractionControl.RemoveInteraction(answer));
            ScrollOffset = Math.Max(ScrollOffset, ScrollBottom);
        }
    }
}
