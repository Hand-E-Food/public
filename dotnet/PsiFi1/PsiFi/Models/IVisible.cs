namespace PsiFi.Models
{
    /// <summary>
    /// A object visible on the <see cref="Map"/>.
    /// </summary>
    interface IVisible
    {
        /// <summary>
        /// This object's appearance.
        /// </summary>
        Appearance Appearance { get; }
        /// <summary>
        /// Raised after this object's appearance is changed.
        /// </summary>
        event SimpleEventHandler? AppearanceChanged;
    }
}
