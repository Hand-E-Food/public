using PsiFi.Abilities;

namespace PsiFi
{
    /// <summary>
    /// A mission.
    /// </summary>
    public class Mission
    {
        /// <summary>
        /// The ability that quits the mission.
        /// </summary>
        public QuitAbility QuitAbility { get; } = new();

        /// <summary>
        /// Creats a new <see cref="Mission"/>.
        /// </summary>
        /// <param name="entryRoom">This mission's entry room.</param>
        public Mission(Room entryRoom)
        {
            EntryRoom = entryRoom;
        }

        /// <summary>
        /// This mission's entry room.
        /// </summary>
        public Room EntryRoom { get; }

    }
}
