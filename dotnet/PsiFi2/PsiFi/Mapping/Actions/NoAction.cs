using PsiFi.ConsoleForms;

namespace PsiFi.Mapping.Actions
{
    /// <summary>
    /// The action of nothing happening.
    /// </summary>
    public class NoAction : IAction
    {
        private readonly string? message;

        /// <param name="message">The message to log.</param>
        public NoAction(string message)
        {
            this.message = message;
        }

        /// <inheritdoc/>
        public void Perform(MapScreen mapScreen)
        {
            if (!string.IsNullOrEmpty(message))
                mapScreen.Log(message);
        }
    }
}
