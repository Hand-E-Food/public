using PsiFi.Models;
using PsiFi.Models.Mapping.Actors.Mobs;

namespace PsiFi.Engines
{
    /// <summary>
    /// Generates a random map.
    /// </summary>
    interface IMapGenerator
    {
        /// <summary>
        /// Creates a map.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The map.</returns>
        Map CreateMap(Player player);
    }
}
