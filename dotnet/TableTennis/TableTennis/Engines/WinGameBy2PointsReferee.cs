using System;
using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// A game referee that requires a player to win by two points.
    /// </summary>
    internal class WinGameBy2PointsReferee : IGameReferee
    {

        /// <summary>
        /// The number of points required to win this game.
        /// </summary>
        public int Goal { get; private set; }

        /// <summary>
        /// Initialises a new instance of a <see cref="WinGameBy2PointsReferee"/> class.
        /// </summary>
        /// <param name="goal">The number of points required to win each game.</param>
        /// <exception cref="ArgumentException">Goal is zero, negative or even.</exception>
        public WinGameBy2PointsReferee(int goal)
        {
            if (goal < 1)
                throw new ArgumentException("goal is zero or negative.", "goal");
            if (goal % 2 == 0)
                throw new ArgumentException("goal is even.", "goal");
            Goal = goal;
        }

        public int Winner(Game game)
        {
            if (game.Player1Score >= Goal && game.Player1Score > game.Player2Score + 1)
                return 1;
            else if (game.Player2Score >= Goal && game.Player2Score > game.Player1Score + 1)
                return 2;
            else
                return 0;
        }
    }
}
