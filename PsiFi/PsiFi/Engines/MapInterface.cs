using PsiFi.Models;
using PsiFi.Models.Mapping;

namespace PsiFi.Engines
{
    /// <summary>
    /// An interface for an actor to view and interact with the map.
    /// </summary>
    class MapInterface
    {
        private readonly Actor actor;

        /// <summary>
        /// The map.
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Initialises a new <see cref="MapInterface"/> object.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="actor">The actor interacting with the map.</param>
        public MapInterface(Map map, Actor actor)
        {
            Map = map;
            this.actor = actor;
        }

        /// <summary>
        /// Requests the actor interacts with this interface.
        /// </summary>
        public void Interact() => actor.Interact(this);
    }
}
