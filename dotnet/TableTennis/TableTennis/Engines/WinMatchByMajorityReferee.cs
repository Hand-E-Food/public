using System;
using System.Linq;
using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// Decides the winner of a match as the player that has won a majority of the match's games.
    /// </summary>
    internal class WinMatchByMajorityReferee : IMatchReferee
    {
        /// <summary>
        /// The referee used to decide the winner of each game of a match.
        /// </summary>
        public IGameReferee GameReferee { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="WinMatchByMajorityReferee"/> class.
        /// </summary>
        /// <param name="gameReferee">The <see cref="IGameReferee"/> used to decide the winner of each game
        /// of a match.</param>
        /// <exception cref="ArgumentNullException">gameReferee is null.</exception>
        public WinMatchByMajorityReferee(IGameReferee gameReferee)
        {
            if (gameReferee == null)
                throw new ArgumentNullException("gameReferee");

            GameReferee = gameReferee;
        }

        public int Winner(Match match)
        {
            int threshold = match.Games.Length / 2;

            int[] wins = new[] { 0, 0, 0 };
            foreach (var game in match.Games)
                wins[GameReferee.Winner(game)]++;

            return Enumerable.Range(1, 2).FirstOrDefault(i => wins[i] > threshold);
        }
    }
}
