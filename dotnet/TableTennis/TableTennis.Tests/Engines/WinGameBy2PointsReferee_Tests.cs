using System;
using System.Collections.Generic;
using TableTennis.Models;
using Xunit;

namespace TableTennis.Engines
{
    public class WinGameBy2PointsReferee_Tests
    {
        [Fact(DisplayName = "WinGameBy2PointsReferee.ctor(goal: 11)")]
        public void ctor_populates_Goal()
        {
            int goal = 11;

            var target = new WinGameBy2PointsReferee(goal);

            int expected = goal;
            int actual = target.Goal;
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "WinGameBy2PointsReferee.ctor"),
            InlineData(0),
            InlineData(-3),
            InlineData(2)]
        public void ctor_throws_ArgumentException_for_invalid_Goal(int goal)
        {
            var ex = Assert.Throws<ArgumentException>(() => new WinGameBy2PointsReferee(goal));

            string expected = "goal";
            string actual = ex.ParamName;
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "WinGameBy2PointsReferee.Winner"), MemberData("Winner_selects_correct_winner_Data")]
        public void Winner_selects_correct_winner(int expected, int goal, int player1Points, int player2Points)
        {
            var game = new Game { Player1Score = player1Points, Player2Score = player2Points };
            var target = new WinGameBy2PointsReferee(goal);

            int actual = target.Winner(game);

            Assert.Equal(expected, actual);
        }
        public static IEnumerable<object[]> Winner_selects_correct_winner_Data()
        {
            return new[] {
                new object[] { 0, 3, 0, 0 },
                new object[] { 0, 3, 1, 0 },
                new object[] { 0, 3, 2, 0 },
                new object[] { 1, 3, 3, 0 },
                new object[] { 0, 3, 0, 1 },
                new object[] { 0, 3, 0, 2 },
                new object[] { 2, 3, 0, 3 },
                new object[] { 0, 3, 2, 1 },
                new object[] { 0, 3, 1, 2 },
                new object[] { 0, 3, 3, 2 },
                new object[] { 0, 3, 2, 3 },
                new object[] { 0, 3, 3, 3 },
                new object[] { 1, 3, 3, 1 },
                new object[] { 2, 3, 1, 3 },
                new object[] { 1, 3, 4, 2 },
                new object[] { 2, 3, 2, 4 },
                new object[] { 0, 9, 8, 8 },
                new object[] { 1, 9, 9, 7 },
                new object[] { 2, 9, 7, 9 },
            };
        }
    }
}
