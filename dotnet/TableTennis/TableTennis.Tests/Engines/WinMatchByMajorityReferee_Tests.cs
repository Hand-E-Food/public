using System;
using Moq;
using Xunit;

namespace TableTennis.Engines
{
    public class WinMatchByMajorityReferee_Tests
    {
        [Fact(DisplayName = "")]
        public void ctor_populates_GameReferee()
        {
            IGameReferee gameReferee = new Mock<IGameReferee>().Object;

            var target = new WinMatchByMajorityReferee(gameReferee);

            var expected = gameReferee;
            var actual = target.GameReferee;
            Assert.Same(expected, actual);
        }

        [Fact(DisplayName = "")]
        public void ctor_throws_ArgumentNullException_when_GameReferee_is_null()
        {
            IGameReferee gameReferee = null;

            var ex = Assert.Throws<ArgumentNullException>(() => new WinMatchByMajorityReferee(gameReferee));

            var expected = "gameReferee";
            var actual = ex.ParamName;
            Assert.Equal(expected, actual);
        }

        //TODO: Test WinMatchByMajorityReferee.Winner
    }
}
