using System;

namespace TableTennis.Models
{
    /// <summary>
    /// A match within a round robin tournament.
    /// </summary>
    public class Match
    {

        /// <summary>
        /// The games in this match.
        /// </summary>
        public Game[] Games { get; private set; }

        /// <summary>
        /// The player from team 1.
        /// </summary>
        public Player Player1 { get; set; }

        /// <summary>
        /// The player from team 2.
        /// </summary>
        public Player Player2 { get; set; }

        /// <summary>
        /// Initialises a new match between two players.
        /// </summary>
        /// <param name="player1">The player from team 1.</param>
        /// <param name="player2">The player from team 2.</param>
        /// <param name="games">The number of games in this match.</param>
        /// <exception cref="ArgumentNullException">player1 or player2 is null.</exception>
        /// <exception cref="ArgumentException">games is negative or an even number.</exception>
        public Match(Player player1, Player player2, int games)
        {
            if (player1 == null)
                throw new ArgumentNullException("player1");
            if (player2 == null)
                throw new ArgumentNullException("player2");
            if (games < 1 || games % 2 != 1)
                throw new ArgumentException("games is negative or an even number.", "games");

            Games = CreateGames(games);
            Player1 = player1;
            Player2 = player2;
        }

        /// <summary>
        /// Initialises an array of games.
        /// </summary>
        /// <param name="count">The number of games to create.</param>
        /// <returns>An initialised array of games.</returns>
        private static Game[] CreateGames(int count)
        {
            Game[] games = new Game[count];
            for (int i = 0; i < count; i++)
                games[i] = new Game();
            return games;
        }
    }
}