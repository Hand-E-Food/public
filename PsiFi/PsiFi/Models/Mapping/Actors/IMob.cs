namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// An actor with a physical presence.
    /// </summary>
    interface IMob : IActor
    {
        /// <summary>
        /// This mob's appearance.
        /// </summary>
        Appearance Appearance { get; }

        /// <summary>
        /// The cell this mob is currently in.
        /// </summary>
        Cell Cell { get; set; }

        /// <summary>
        /// This mob's health.
        /// </summary>
        Range Health { get; }
    }
}
