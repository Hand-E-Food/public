using System;
using Xunit;

namespace TableTennis.Models
{
    public class Match_ctor_Tests
    {

        [Fact(DisplayName = "Match.ctor(games: 3)")]
        public void Populates_properties()
        {
            Player player1 = new Player();
            Player player2 = new Player();
            int games = 3;

            Match target = new Match(player1, player2, games);

            Assert.Same(player1, target.Player1);
            Assert.Same(player2, target.Player2);
            Assert.Equal(games, target.Games.Length);
        }

        [Theory(DisplayName = ""),
            InlineData(0),
            InlineData(-1)]
        public void Throws_ArgumentException_when_games_is_0_or_negative(int games)
        {
            Player player1 = new Player();
            Player player2 = new Player();

            var ex = Assert.Throws<ArgumentException>(() => new Match(player1, player2, games));

            Assert.Equal("games", ex.ParamName);
        }
    }
}
