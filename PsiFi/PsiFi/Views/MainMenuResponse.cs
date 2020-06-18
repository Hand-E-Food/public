namespace PsiFi.Views
{
    /// <summary>
    /// The response from the main menu.
    /// </summary>
    class MainMenuResponse
    {
        /// <summary>
        /// The selected aciton.
        /// </summary>
        public MainMenuAction Action { get; private set; }

        private MainMenuResponse() { }

        public static readonly MainMenuResponse NewGame = new MainMenuResponse
        {
            Action = MainMenuAction.NewGame,
        };

        public static readonly MainMenuResponse Quit = new MainMenuResponse
        {
            Action = MainMenuAction.Quit,
        };
    }
}