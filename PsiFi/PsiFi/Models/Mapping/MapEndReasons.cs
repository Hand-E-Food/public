using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// A collection of reasons why a map is ending.
    /// </summary>
    class MapEndReasons
    {
        /// <summary>
        /// Indicates the player quit the game.
        /// </summary>
        public static readonly MapEndReasons Quit = new MapEndReasons(MapEndReason.Quit);

        private readonly MapEndReason[] endReasons;

        private MapEndReasons(MapEndReason endReason)
        {
            endReasons = new[] { endReason };
        }

        public MapEndReasons(IEnumerable<MapEndReason> endReasons)
        {
            this.endReasons = endReasons
                .Where(endReason => endReason != null)
                .ToArray();
        }

        /// <summary>
        /// True if there is no reason to end the map. False if there is a reason to end the map.
        /// </summary>
        public bool None => endReasons.Length == 0;

        /// <summary>
        /// Checks if the specified <paramref name="reason"/> contributed to ending this map.
        /// </summary>
        /// <param name="reason">The reason to check.</param>
        /// <returns>True if the specified <paramref name="reason"/> is a reason this map ended. Otherwise false.</returns>
        public bool Contains(MapEndReason reason) => endReasons.Contains(reason);
    }
}
