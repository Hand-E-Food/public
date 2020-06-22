using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Geometry;

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

        /// <summary>
        /// Attempts to move the actor one step in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to move.</param>
        /// <returns>True if the move succeeded; otherwise false.</returns>
        public bool Move(Direction direction)
        {
            if (!(actor is Mob mob))
                return false;

            var cell = Map[mob.Cell.Location + direction];
            if (!cell.Terrain.AllowsMovement || cell.Mob != null)
                return false;

            mob.Cell = cell;
            return true;
        }
    }
}
