using ConsoleForms;

namespace PsiFi.ConsoleView.Forms
{
    /// <summary>
    /// Displays a mission.
    /// </summary>
    internal class MissionControl : DockPanel
    {
        /// <summary>
        /// The control that displays log messages.
        /// </summary>
        public LogControl LogControl { get; }

        /// <summary>
        /// The control that displays the protagonist, and mission-level abilities.
        /// </summary>
        public MissionPlayerControl MissionPlayerControl { get; }

        /// <summary>
        /// The control that displays the room.
        /// </summary>
        public RoomControl RoomControl { get; }

        /// <summary>
        /// Creates a new <see cref="MissionControl"/>.
        /// </summary>
        public MissionControl()
        {
            LogControl = new() {
                AnchorLeft = 0,
                AnchorRight = 0,
                AnchorBottom = 0,
                DesiredHeight = 5,
            };
            Children.Add(LogControl);

            MissionPlayerControl = new() {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorBottom = 0,
            };
            Children.Add(MissionPlayerControl);

            RoomControl = new() {
                AnchorLeft = 0,
                AnchorTop = 0,
                AnchorRight = 0,
                AnchorBottom = 0,
            };
            Children.Add(RoomControl);
        }

        /// <summary>
        /// The displayed game.
        /// </summary>
        public Game? Game
        {
            get => game;
            set
            {
                if (game == value) return;
                game = value;
                MissionPlayerControl.Protagonist = game?.Protagonist;
            }
        }
        private Game? game = null;
    }
}
