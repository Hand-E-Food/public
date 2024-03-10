namespace PsiFi.Interactions
{
    /// <summary>
    /// Requests the UI to display the specified room.
    /// </summary>
    public class ShowRoomInteraction : Interaction
    {
        /// <summary>
        /// Creates a new <see cref="ShowRoomInteraction"/>.
        /// </summary>
        /// <param name="room">The room to show.</param>
        public ShowRoomInteraction(Room room)
        {
            Room = room;
        }

        /// <summary>
        /// The room to show.
        /// </summary>
        public Room Room { get; }
    }
}
