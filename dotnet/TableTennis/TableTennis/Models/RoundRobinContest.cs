using System;
using System.Collections.Generic;
using System.Linq;

namespace TableTennis.Models
{
    /// <summary>
    /// A table tennis round robin tournament.
    /// </summary>
    public class RoundRobinContest : Contest
    {
        /// <summary>
        /// Given two teams with so many members, details in what order each player from each team plays.
        /// </summary>
        #region protected static readonly Dictionary<Tuple<int, int>, Tuple<int, int>[]> PlayerOrder = new Dictionary<Tuple<int, int>, Tuple<int, int>[]> { ... };
        protected static readonly Dictionary<Tuple<int, int>, Tuple<int, int>[]> PlayerOrder = new Dictionary<Tuple<int, int>, Tuple<int, int>[]>
        {
            { T(1,1), new[] { T(0,0) } },
            { T(1,2), new[] { T(0,0), T(0,1)} },
            { T(1,3), new[] { T(0,0), T(0,1), T(0,2)} },
            { T(2,1), new[] { T(0,0), T(1,0) } },
            { T(2,2), new[] { T(0,0), T(1,1), T(0,1), T(1,0) } },
            { T(2,3), new[] { T(0,0), T(1,1), T(0,2), T(1,0), T(0,1), T(1,2) } },
            { T(3,1), new[] { T(0,0), T(1,0), T(2,0) } },
            { T(3,2), new[] { T(0,0), T(1,1), T(2,0), T(0,1), T(1,0), T(2,1) } },
            { T(3,3), new[] { T(0,0), T(1,1), T(2,2), T(1,0), T(0,2), T(2,1), T(1,2), T(2,0), T(0,1) } },
        };
        #endregion

        /// <summary>
        /// The players in team 1.
        /// </summary>
        public Team Team1 { get; private set; }

        /// <summary>
        /// The players in team 2.
        /// </summary>
        public Team Team2 { get; private set; }

        /// <summary>
        /// Initialises a new table tennis match.
        /// </summary>
        /// <param name="team1">Team 1.</param>
        /// <param name="team2">Team 2.</param>
        /// <param name="games">The number of games in each match.</param>
        /// <exception cref="ArgumentNullException">team1 or team2 is null.</exception>
        /// <exception cref="ArgumentException">team1 or team2 is empty; or games is negative or even.</exception>
        public RoundRobinContest(Team team1, Team team2, int games)
        {
            if (team1 == null)
                throw new ArgumentNullException("team1");
            if (team2 == null)
                throw new ArgumentNullException("team2");

            Team1 = team1;
            Team2 = team2;

            CreateMatches(games);
        }

        /// <summary>
        /// Creates a set of matches pairing each member of team 1 with each member of team 2.
        /// </summary>
        /// <param name="games">The number of games in each match.</param>
        private void CreateMatches(int games)
        {
            var pairs = PlayerOrder[T(Team1.Players.Length, Team2.Players.Length)];
            Matches = new Match[pairs.Length];
            for (int i = 0; i < pairs.Length; i++)
                Matches[i] = new Match(Team1.Players[pairs[i].Item1], Team2.Players[pairs[i].Item2], games);
        }

        /// <summary>
        /// Shorthand to create a new <see cref="Tuple{Int32, Int32}"/>.
        /// </summary>
        /// <param name="item1">The value of the tuple's first component.</param>
        /// <param name="item2">The value of the tuple's second component.</param>
        /// <returns>A <see cref="Tuple{Int32, Int32}"/> initialised with the specified items.</returns>
        private static Tuple<int, int> T(int item1, int item2)
        {
            return new Tuple<int, int>(item1, item2);
        }
    }
}
