namespace PsiFi
{
    /// <summary>
    /// A room in a map.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// The mobs in this room.
        /// </summary>
        public List<Mob> Mobs { get; } = new();

        /// <summary>
        /// The mobs other than the protagonist.
        /// </summary>
        public IEnumerable<Mob> OtherMobs => Mobs.Where(mob => mob.GetType() != typeof(Protagonist));
    }
}
