using System.Collections.Generic;
using Xunit;

namespace TableTennis.Engines
{
    public class TeamInputValidation_IsValid_Tests
    {
        [Theory(DisplayName = "TeamInputValidation.IsValid"), MemberData("IsValid_returns_expected_value_Data")]
        public void IsValid_returns_expected_value(bool expected, string team1Name, string team1Player1Name, string team2Name, string team2Player2Name)
        {
            IValidation target = new TeamInputValidation(
                () => team1Name,
                () => team1Player1Name,
                () => team2Name,
                () => team2Player2Name);

            bool actual = target.IsValid();

            Assert.Equal(expected, actual);
        }
        public static IEnumerable<object[]> IsValid_returns_expected_value_Data()
        {
            return new[] {
                new object[] { true , "Text", "Text", "Text", "Text" },
                new object[] { false, ""    , "Text", "Text", "Text" },
                new object[] { false, null  , "Text", "Text", "Text" },
                new object[] { false, "Text", ""    , "Text", "Text" },
                new object[] { false, "Text", null  , "Text", "Text" },
                new object[] { false, "Text", "Text", ""    , "Text" },
                new object[] { false, "Text", "Text", null  , "Text" },
                new object[] { false, "Text", "Text", "Text", ""     },
                new object[] { false, "Text", "Text", "Text", null   },
            };
        }
    }
}
