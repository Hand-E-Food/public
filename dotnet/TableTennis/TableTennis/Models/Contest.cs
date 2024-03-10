namespace TableTennis.Models
{
    /// <summary>
    /// A contest of one or more matches.
    /// </summary>
    public abstract class Contest
    {
        /// <summary>
        /// The matches in the contest.
        /// </summary>
        public Match[] Matches { get; protected set; }
    }
}