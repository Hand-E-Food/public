namespace PsiFi.Interactions
{
    /// <summary>
    /// Requests the UI to display the mission screen.
    /// </summary>
    public class StartMissionInteraction : Interaction
    {
        /// <summary>
        /// The mission's player interactions.
        /// </summary>
        public IEnumerable<Interaction> Interactions { get; }

        /// <summary>
        /// Creates a new <see cref="StartMissionInteraction"/>.
        /// </summary>
        /// <param name="interactions">The mission's player interactions.</param>
        public StartMissionInteraction(IEnumerable<Interaction> interactions)
        {
            Interactions = interactions;
        }
    }
}
