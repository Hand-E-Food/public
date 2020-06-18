using PsiFi.Models;
using PsiFi.Models.Mapping;

namespace PsiFi.Engines
{
    /// <summary>
    /// An interface for an actor to view and interact with the map.
    /// </summary>
    class MapInterface
    {
        private readonly IActor actor;

        /// <summary>
        /// The map.
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Initialises a new <see cref="MapInterface"/> object.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="actor">The actor interacting with the map.</param>
        public MapInterface(Map map, IActor actor)
        {
            Map = map;
            this.actor = actor;
        }
    }
}
