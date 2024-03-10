using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// A referee that decides the winner of a match.
    /// </summary>
    public interface IMatchReferee
    {
        /// <summary>
        /// Gets the referee that decides the winner of each game.
        /// </summary>
        IGameReferee GameReferee { get; }

        /// <summary>
        /// Decides the winner of the specified match.
        /// </summary>
        /// <param name="match">The match to decide.</param>
        /// <returns>1 or 2 if that player has won this match, or 0 if no one has won this match.</returns>
        int Winner(Match match);
    }
}