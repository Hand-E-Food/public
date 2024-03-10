using PsiFi.ConsoleView.Forms;
using PsiFi.Interactions;

namespace PsiFi.ConsoleView
{
    /// <summary>
    /// Views a game.
    /// </summary>
    internal class HomeView : View<HomeControl>
    {
        /// <summary>
        /// Creates a new <see cref="HomeView"/>.
        /// </summary>
        /// <param name="game">The game.</param>
        public HomeView(Game game)
        {
            Game = game;
        }

        protected void Interact(StartMissionInteraction interaction)
        {
            var missionView = new MissionView(Game);
            missionView.Run(TopLevelControl.Canvas!, interaction.Interactions);
        }

        /// <summary>
        /// The game.
        /// </summary>
        public Game Game { get; }
    }
}
