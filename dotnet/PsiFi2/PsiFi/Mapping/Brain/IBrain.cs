namespace PsiFi.Mapping.Brain
{
    /// <summary>
    /// A mob's brain.
    /// </summary>
    public abstract class Brain
    {
        /// <summary>
        /// Gets the map this brain's mob is on.
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Gets the mob owning this brain.
        /// </summary>
        public Mob Mob { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mob"></param>
        /// <param name="map"></param>
        public Brain(Mob mob, Map map)
        {
            Mob = mob;
            Map = map;
        }
    }
}
