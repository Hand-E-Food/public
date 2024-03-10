namespace PsiFi.Interactions
{
    /// <summary>
    /// An interaction requesting the player selects one of the specified options.
    /// </summary>
    /// <typeparam name="T">The type of the options.</typeparam>
    public class SelectionInteraction<T> : Interaction
    {
        /// <summary>
        /// Creates a new <see cref="SelectionInteraction"/>.
        /// </summary>
        /// <typeparam name="T">The type of the options.</typeparam>
        /// <param name="options">The options to select from.</param>
        /// <param name="message">The message to display.</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="options"/> is empty.</exception>
        public SelectionInteraction(IEnumerable<T> options, string? message = null)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            Options = options.ToArray();
            if (Options.Count == 0) throw new ArgumentException("At least one option must be specified.", nameof(options));
            Message = message;
        }

        /// <summary>
        /// The message to display.
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// The options to select from.
        /// </summary>
        public ICollection<T> Options { get; }

        /// <summary>
        /// The object the player selected.
        /// <see langword="null"/> if no option was selected.
        /// </summary>
        public T? Response { get; set; }
    }
}
