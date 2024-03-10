namespace PsiFi.Interactions
{
    /// <summary>
    /// An interaction to wait for the player's acknowledgement.
    /// </summary>
    public class OkInteraction : NotificationInteraction
    {
        /// <summary>
        /// Creates a new <see cref="OkInteraction"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/></exception>
        public OkInteraction(string message) : base(message) { }
    }
}
