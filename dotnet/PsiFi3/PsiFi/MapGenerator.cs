using PsiFi.Mobs;

namespace PsiFi
{
    /// <summary>
    /// Generates maps.
    /// </summary>
    internal class MapGenerator
    {
        /// <summary>
        /// Creates a new map.
        /// </summary>
        /// <returns>The map's entry room.</returns>
        public Room CreateMap()
        {
            Room room = new();
            new Rat() { Room = room };
            return room;
        }
    }
}
