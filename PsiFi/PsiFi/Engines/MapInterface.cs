using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Geometry;
using PsiFi.Models.Mapping.Items;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace PsiFi.Engines
{
    /// <summary>
    /// An interface for an actor to view and interact with the map.
    /// </summary>
    class MapInterface
    {
        private readonly Actor actor;

        /// <summary>
        /// True if the player is quitting the game. Otherwise, false.
        /// </summary>
        public bool IsQuitting { get; private set; } = false;

        /// <summary>
        /// The map.
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// The random number generator.
        /// </summary>
        public IRandom Random { get; }

        /// <summary>
        /// Causes the <paramref name="actor"/> to interact with a <paramref name="map"/>.
        /// </summary>
        /// <param name="actor">The actor interacting with the map.</param>
        /// <param name="map">The map.</param>
        /// <param name="random">The random number generator.</param>
        public static MapInterface Interact(Actor actor, Map map, IRandom random)
        {
            var mapInterface = new MapInterface(actor, map, random);
            actor.Interact(mapInterface);
            return mapInterface;
        }

        private MapInterface(Actor actor, Map map, IRandom random)
        {
            Map = map;
            this.actor = actor;
            Random = random;
        }

        /// <summary>
        /// Attempts to attack the target with the weapon.
        /// </summary>
        /// <param name="target">The target to attack.</param>
        /// <param name="weapon">The weapon to attack with.</param>
        /// <returns>
        /// True if the attack was possible.
        /// False if the attack is impossible.
        /// </returns>
        public bool Attack(Mob target, Weapon weapon)
        {
            if (actor is Mob mob && !weapon.AttackRange.Contains(mob.Cell.Location.DistanceFrom(target.Cell.Location)))
                return false;

            var damages = weapon.Damage
                .Select(Random.Next);

            if (weapon.Damage.Length > 1)
                damages = damages
                    .GroupBy(damage => damage.Type)
                    .Select(group => new Damage(group.Sum(damage => damage.Amount), group.Key));

            target.Health.Value -= damages.Sum(damage => damage.Amount);
            if (target.Health.Value <= 0)
            {
                target.Die();
                Map.Actors.Remove(target);
            }

            return true;
        }

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

        /// <summary>
        /// Quits the game.
        /// </summary>
        /// <returns>false</returns>
        public bool Quit()
        {
            IsQuitting = true;
            return false;
        }
    }
}
