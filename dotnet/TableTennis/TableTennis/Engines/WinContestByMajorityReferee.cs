using System;
using System.Linq;
using TableTennis.Models;

namespace TableTennis.Engines
{
    /// <summary>
    /// Decides the winner of a contest as the team that wins more a majority of the contest's games.
    /// </summary>
    internal class WinContestByMajorityReferee : IContestReferee
    {
        /// <summary>
        /// The referee that decides the winner of each match of a contest.
        /// </summary>
        public IMatchReferee MatchReferee { get; private set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="WinContestByMajorityReferee"/> class.
        /// </summary>
        /// <param name="matchReferee">The <see cref="IMatchReferee"/> that decides the winner of each match
        /// in a contest.</param>
        /// <exception cref="ArgumentNullException">matchReferee is null.</exception>
        public WinContestByMajorityReferee(IMatchReferee matchReferee)
        {
            if (matchReferee == null)
                throw new ArgumentNullException("matchReferee");

            MatchReferee = matchReferee;
        }

        public int Winner(Contest contest)
        {
            int threshold = contest.Matches.Length / 2;

            int[] wins = new[] { 0, 0, 0 };
            foreach (var match in contest.Matches)
                wins[MatchReferee.Winner(match)]++;

            return Enumerable.Range(1, 2).FirstOrDefault(i => wins[i] > threshold);
        }
    }
}
