using Xunit;

namespace WordleSolver.UnitTests
{
    public class Clues_Tests
    {
        [Theory]
        [InlineData(0, "00000")]
        [InlineData(1, "00001")]
        [InlineData(2, "00002")]
        [InlineData(81, "10000")]
        [InlineData(162, "20000")]
        [InlineData(121, "11111")]
        [InlineData(242, "22222")]
        [InlineData(173, "20102")]
        public void FromHashCode_ReturnsCorrectValue(int hashCode, Clues expected)
        {
            Clues actual = Clues.FromHashCode(hashCode);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("00000",   0)]
        [InlineData("00001",   1)]
        [InlineData("00002",   2)]
        [InlineData("10000",  81)]
        [InlineData("20000", 162)]
        [InlineData("11111", 121)]
        [InlineData("22222", 242)]
        [InlineData("20102", 173)]
        public void GetHashCode_ReturnsCorrectValue(Clues clues, int expected)
        {
            int actual = clues.GetHashCode();
            Assert.Equal(expected, actual);
        }
    }
}
