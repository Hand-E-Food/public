using System.Collections.Generic;
using System.Linq;

namespace TableTennis.Models
{
    internal class TeamInput
    {
        public string TeamName { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string Player3Name { get; set; }

        public IEnumerable<string> PlayerNames => new[] { Player1Name, Player2Name, Player3Name }.Where(p => !string.IsNullOrWhiteSpace(p));

        public Team CreateTeam() => new Team(TeamName, PlayerNames.Select(name => new Player(name)));
    }
}
