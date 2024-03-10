using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// Decides the winner of a game.
    /// </summary>
    public interface IGameReferee
    {
        /// <summary>
        /// Decides the winner of the specified game.
        /// </summary>
        /// <param name="game">The game to decide.</param>
        /// <returns>Returns 1 or 2 if that player has won this game, or 0 if no one has won this game.</returns>
        int Winner(Game game);
    }
}