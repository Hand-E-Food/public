namespace TableTennis.Models
{
    /// <summary>
    /// A table tennis player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The player's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The player's address and phone number.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Initialises a new, blank player.
        /// </summary>
        public Player()
        {
        }

        /// <summary>
        /// Initialises a new player.
        /// </summary>
        /// <param name="name">The player's name.</param>
        /// <param name="details">The player's address and phone number.</param>
        public Player(string name, string details = null)
        {
            Name = name;
            Details = details;
        }
    }
}