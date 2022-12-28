namespace PsiFi.Interactions
{
    /// <summary>
    /// An interaction that displays a message to the player and requires no response.
    /// </summary>
    public class NotificationInteraction : Interaction
    {
        /// <summary>
        /// Creates a new <see cref="NotificationInteraction"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/></exception>
        public NotificationInteraction(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <summary>
        /// The message to display.
        /// </summary>
        public string Message { get; }
    }
}
