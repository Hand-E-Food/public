namespace PsiFi.Interactions
{
    /// <summary>
    /// An interaction to get a yes/no answer from the player.
    /// </summary>
    public class YesNoInteraction : NotificationInteraction
    {
        /// <summary>
        /// Creates a new <see cref="YesNoInteraction"/>.
        /// </summary>
        /// <param name="message">The message to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is <see langword="null"/></exception>
        public YesNoInteraction(string message) : base(message) { }

        /// <summary>
        /// True if the player responded "yes".
        /// False if the player responded "no".
        /// </summary>
        public bool Response { get; set; }
    }
}
