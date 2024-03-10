using System;
using Moq;
using Xunit;

namespace TableTennis.Engines
{
    public class WinContestByMajorityReferee_Tests
    {
        [Fact(DisplayName = "")]
        public void ctor_populates_MatchReferee()
        {
            IMatchReferee matchReferee = new Mock<IMatchReferee>().Object;

            var target = new WinContestByMajorityReferee(matchReferee);

            var expected = matchReferee;
            var actual = target.MatchReferee;
            Assert.Same(expected, actual);
        }

        [Fact(DisplayName = "")]
        public void ctor_throws_ArgumentNullException_when_MatchReferee_is_null()
        {
            IMatchReferee matchReferee = null;

            var ex = Assert.Throws<ArgumentNullException>(() => new WinContestByMajorityReferee(matchReferee));

            var expected = "matchReferee";
            var actual = ex.ParamName;
            Assert.Equal(expected, actual);
        }

        //TODO: Test WinContestByMajorityReferee.Winner
    }
}
