using PsiFi.Models;

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
        /// <returns>The map.</returns>
        Map CreateMap();
    }
}
