using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// Decides the winner of a contest.
    /// </summary>
    public interface IContestReferee
    {
        /// <summary>
        /// Gets the referee that decides the winner of each match.
        /// </summary>
        IMatchReferee MatchReferee { get; }

        /// <summary>
        /// Decides the winner of the specified contest.
        /// </summary>
        /// <param name="contest">The contest to decide.</param>
        /// <returns>1 or 2 if that team has won this contest, or 0 if no team has won this contest.</returns>
        int Winner(Contest contest);
    }
}