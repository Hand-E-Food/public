using PsiFi.Models;

namespace PsiFi.Views
{
    /// <summary>
    /// Displays a map.
    /// </summary>
    interface IMapView
    {
        /// <summary>
        /// The map to display.
        /// </summary>
        Map Map { get; set; }

        /// <summary>
        /// Clears the message.
        /// </summary>
        void ClearMessage();

        /// <summary>
        /// Displays a message.
        /// </summary>
        void Message(string message);

        /// <summary>
        /// Draws all cells that have changed since last being drawn.
        /// </summary>
        void Update();

        /// <summary>
        /// Gets input from the player.
        /// </summary>
        /// <returns>The player's input.</returns>
        MapViewResponse GetPlayerInput();
    }
}
