using System;
using System.Collections.Generic;
using Xunit;

namespace TableTennis.Models
{
    public class RoundRobinContest_ctor_Tests
    {
        [Fact(DisplayName = "RoundRobinContest.ctor")]
        public void Populates_properties()
        {
            Team team1 = new Team("Team 1", new[] { new Player() });
            Team team2 = new Team("Team 2", new[] { new Player() });
            int games = 3;
            RoundRobinContest target = new RoundRobinContest(team1, team2, games);
            Assert.Same(team1, target.Team1);
            Assert.Same(team2, target.Team2);
        }

        [Fact(DisplayName = "RoundRobinContest.ctor(team1: null)")]
        public void Throws_AgrumentNullException_when_Team1_is_null()
        {
            Team team1 = null;
            Team team2 = new Team("Team 2", new[] { new Player() });
            int games = 3;

            var ex = Assert.Throws<ArgumentNullException>(() => new RoundRobinContest(team1, team2, games));

            Assert.Equal("team1", ex.ParamName);
        }

        [Fact(DisplayName = "RoundRobinContest.ctor(team2: null)")]
        public void Throws_AgrumentNullException_when_Team2_is_null()
        {
            Team team1 = new Team("Team 1", new[] { new Player() });
            Team team2 = null;
            int games = 3;

            var ex = Assert.Throws<ArgumentNullException>(() => new RoundRobinContest(team1, team2, games));

            Assert.Equal("team2", ex.ParamName);
        }

        [Theory(DisplayName = "RoundRobinContest.ctor"),
            InlineData(0),
            InlineData(-1)]
        public void Throws_AgrumentException_when_Games_is_0_or_negative(int games)
        {
            Team team1 = new Team("Team 1", new[] { new Player() });
            Team team2 = new Team("Team 2", new[] { new Player() });

            var ex = Assert.Throws<ArgumentException>(() => new RoundRobinContest(team1, team2, games));

            Assert.Equal("games", ex.ParamName);
        }

        [Theory(DisplayName = "RoundRobinContest.ctor"), MemberData("Creates_Matches_Data")]
        public void Creates_Matches(int team1Size, int team2Size)
        {
            Team team1 = new Team("Team 1", new Player[team1Size]);
            Team team2 = new Team("Team 2", new Player[team2Size]);
            int games = 3;
            for (int i = 0; i < team1Size; i++)
                team1.Players[i] = new Player();
            for (int i = 0; i < team2Size; i++)
                team2.Players[i] = new Player();

            RoundRobinContest target = new RoundRobinContest(team1, team2, games);

            // Assert the correct number of matches were created.
            int expectedMatches = team1Size * team2Size;
            Assert.Equal(expectedMatches, target.Matches.Length);
            var matchPairs = new HashSet<Tuple<Player, Player>>();
            foreach (Match match in target.Matches)
            {
                // Assert Player1 is from Team1, and Player2 is from Team2.
                Assert.Contains(match.Player1, team1.Players);
                Assert.Contains(match.Player2, team2.Players);
                // Assert the games parameter was passed to each match.
                Assert.Equal(games, match.Games.Length);
                // Assert every match pairs different players.
                Assert.True(matchPairs.Add(Tuple.Create(match.Player1, match.Player2)));
            }
        }
        public static IEnumerable<object[]> Creates_Matches_Data
        {
            get
            {
                for (int team1Size = 1; team1Size <= 3; team1Size++)
                    for (int team2Size = 1; team2Size <= 3; team2Size++)
                        yield return new object[] { team1Size, team2Size };
            }
        }
    }
}
