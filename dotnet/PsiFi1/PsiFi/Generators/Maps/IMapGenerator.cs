using PsiFi.Models;

namespace PsiFi.Generators.Maps
{
    interface IMapGenerator
    {
        /// <summary>
        /// Generates a new map.
        /// </summary>
        /// <param name="state">The game's state.</param>
        /// <returns>The map.</returns>
        Map GenerateMap(State state);
    }
}
