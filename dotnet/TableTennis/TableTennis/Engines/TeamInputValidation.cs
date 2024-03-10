using System;

namespace TableTennis.Engines
{
    /// <summary>
    /// Validates the team name inputs.
    /// </summary>
    internal class TeamInputValidation : IValidation
    {
        private readonly Func<string> Team1Name;
        private readonly Func<string> Team1Player1Name;
        private readonly Func<string> Team2Name;
        private readonly Func<string> Team2Player1Name;

        /// <summary>
        /// Initialises a new instance of the <see cref="TeamInputValidation"/> class.
        /// </summary>
        /// <param name="team1Name">A function that gets the input team 1 name.</param>
        /// <param name="team1Player1Name">A function that gets the input team 1, player 1 name.</param>
        /// <param name="team2Name">A function that gets the input team 2 name.</param>
        /// <param name="team2Player1Name">A function that gets the input team 2, player 1 name.</param>
        public TeamInputValidation(
            Func<string> team1Name,
            Func<string> team1Player1Name,
            Func<string> team2Name,
            Func<string> team2Player1Name)
        {
            Team1Name        = team1Name;
            Team1Player1Name = team1Player1Name;
            Team2Name        = team2Name;
            Team2Player1Name = team2Player1Name;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Team1Name())
                && !string.IsNullOrWhiteSpace(Team1Player1Name())
                && !string.IsNullOrWhiteSpace(Team2Name())
                && !string.IsNullOrWhiteSpace(Team2Player1Name());
        }
    }
}
