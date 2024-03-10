using System;
using System.Collections.Generic;
using System.Linq;
using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// Provides functions to transform between data types.
    /// </summary>
    internal static class Transform
    {
        /// <summary>
        /// Converts the <see cref="IList{string}"/> to a team of players.
        /// </summary>
        /// <param name="list">The list of <see cref="string"/> values used to populate the team.</param>
        /// <returns>The initialised team.</returns>
        /// <exception cref="ArgumentException">A team must consist of a name and at least one player.
        /// </exception>
        public static Team ToTeam(IList<string> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (list.Count < 2)
                throw new ArgumentException("A team must consist of a name and at least one player.", "list");

            return new Team(
                list[0],
                list.Skip(1)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => new Player(x))
            );
        }
    }
}
