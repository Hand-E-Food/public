using Xunit;

namespace WordleSolver.UnitTests
{
    public class Solver_Tests
    {
        [Theory]
        [InlineData("abcde", "abcde", "22222")]
        [InlineData("edcba", "abcde", "11211")]
        [InlineData("fghij", "abcde", "00000")]
        [InlineData("rrrrr", "error", "02202")]
        [InlineData("rrree", "error", "12210")]
        public void CalculateClues(string guess, string target, Clues expected)
        {
            var actual = Solver.CalculateClues(guess, target);
            Assert.Equal(expected, actual);
        }
    }
}
