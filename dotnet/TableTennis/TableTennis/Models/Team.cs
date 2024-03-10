using System;
using System.Collections.Generic;
using System.Linq;

namespace TableTennis.Models
{
    /// <summary>
    /// A team of table tennis players.
    /// </summary>
    public class Team
    {
        /// <summary>
        /// The team's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The team's players.
        /// </summary>
        public Player[] Players { get; set; }

        /// <summary>
        /// Initialises a new, empty, table tennis team.
        /// </summary>
        public Team()
        { }

        /// <summary>
        /// Initialises a new table tennis team.
        /// </summary>
        /// <param name="name">The team's name.</param>
        /// <param name="players">The team's players.</param>
        /// <exception cref="ArgumentNullException">name or players is null.</exception>
        /// <exception cref="ArgumentException">A team must have between 1 and 3 players.</exception>
        public Team(string name, IEnumerable<Player> players)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (players == null)
                throw new ArgumentNullException("players");

            Name = name;
            Players = players.ToArray();

            if (Players.Length < 1 || Players.Length > 3)
                throw new ArgumentException("players", "players has less than 1 or more than 3 players.");
        }
    }
}