using System;
using System.Collections.Generic;
using Xunit;

namespace TableTennis.Models
{
    public class Team_ctor_Tests
    {
        [Fact(DisplayName = "Team.ctor(players: null)")]
        public void Throws_ArgumentNullException_when_players_is_null()
        {
            string name = "Unit test name";
            IEnumerable<Player> players = null;
            Assert.Throws<ArgumentNullException>(() => new Team(name, players));
        }

        [Fact(DisplayName = "Team.ctor(players: Player[0])")]
        public void Throws_ArgumentException_when_players_is_empty()
        {
            string name = "Unit test name";
            IEnumerable<Player> players = new Player[0];
            Assert.Throws<ArgumentException>(() => new Team(name, players));
        }

        [Fact(DisplayName = "Team.ctor(players: Player[1])")]
        public void Works_when_players_contains_1_element()
        {
            string name = "Unit test name";
            IEnumerable<Player> players = new[] { new Player() };
            new Team(name, players);
        }

        [Fact(DisplayName = "Team.ctor(players: Player[2])")]
        public void Populates_properties()
        {
            string name = "Unit test name";
            IEnumerable<Player> players = new[] { new Player(), new Player() };
            Team target = new Team(name, players);
            Assert.Equal(name, target.Name);
            Assert.Equal(players, target.Players);
        }

        [Fact(DisplayName = "Team.ctor(players: Player[3])")]
        public void Works_when_players_contains_3_element()
        {
            string name = "Unit test name";
            IEnumerable<Player> players = new[] { new Player(), new Player(), new Player() };
            new Team(name, players);
        }

        [Fact(DisplayName = "Team.ctor(players: Player[4])")]
        public void Throws_ArgumentException_when_players_contains_4_elements()
        {
            string name = "Unit test name";
            IEnumerable<Player> players = new[] { new Player(), new Player(), new Player(), new Player() };
            Assert.Throws<ArgumentException>(() => new Team(name, players));
        }
    }
}
