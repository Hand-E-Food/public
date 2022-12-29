using PsiFi.ConsoleView.Forms;

namespace PsiFi.ConsoleView
{
    internal class MissionView : View<MissionControl>
    {
        /// <summary>
        /// Creates a new <see cref="MissionView"/>.
        /// </summary>
        /// <param name="game">The game to display.</param>
        public MissionView(Game game)
        {
            TopLevelControl.Game = game;
            RegisterInteractions(TopLevelControl.LogControl);
            RegisterInteractions(TopLevelControl.MissionPlayerControl);
            RegisterInteractions(TopLevelControl.RoomControl);
        }
    }
}