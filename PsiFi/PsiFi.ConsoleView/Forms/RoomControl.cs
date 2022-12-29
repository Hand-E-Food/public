using ConsoleForms;
using PsiFi.Interactions;

namespace PsiFi.ConsoleView.Forms
{
    internal class RoomControl : ScrollList
    {
        /// <summary>
        /// The currently displayed room.
        /// </summary>
        public Room? Room { get; private set; }

        public void Interact(ShowRoomInteraction interaction)
        {
            Room = interaction.Room;
        }

        public void Interact(SelectionInteraction<Mob> interaction)
        {
        }
    }
}