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
        /// Draws all cells that have changed since last being drawn.
        /// </summary>
        void Update();
    }
}
