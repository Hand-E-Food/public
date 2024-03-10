using System;
using System.Collections.Generic;
using Xunit;

namespace TableTennis.Engines
{
    public class Transform_ToTeam_Tests
    {
        [Fact(DisplayName = "")]
        public void ToTeam_throws_ArgumentNullException_if_list_is_null()
        {
            string[] list = null;
            var ex = Assert.Throws<ArgumentNullException>(() => Transform.ToTeam(list));

            var expected = "list";
            var actual = ex.ParamName;
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = ""), MemberData("ToTeam_throws_ArgumentException_if_list_has_less_than_two_elements_Data")]
        public void ToTeam_throws_ArgumentException_if_list_has_less_than_two_elements(string[] list)
        {
            var ex = Assert.Throws<ArgumentException>(() => Transform.ToTeam(list));

            var expected = "list";
            var actual = ex.ParamName;
            Assert.Equal(expected, actual);
        }
        public static IEnumerable<object[]> ToTeam_throws_ArgumentException_if_list_has_less_than_two_elements_Data()
        {
            return new[] {
                new object[] { new string[] { } },
                new object[] { new string[] { "Team Name" } },
            };
        }
    }
}
