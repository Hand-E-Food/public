using System;
using Xunit;

namespace GraphSolver
{
    public class Solution_Tests
    {
        [Fact]
        public void EmptySolution_IsSuccessful()
        {
            Assert.True(Solution.Empty.IsSuccessful);
        }

        [Fact]
        public void EmptySolution_IsEmpty()
        {
            var solution = Solution.Empty;
            Assert.Empty(solution.Edges);
            Assert.Empty(solution.Vertices);
            Assert.Equal(0, solution.Weight);
        }

        [Fact]
        public void ImpossibleSolution_IsNotSuccessful()
        {
            Assert.False(Solution.Impossible.IsSuccessful);
        }

        [Fact]
        public void Solution_IsSuccessful()
        {
            var actual = new Solution(Array.Empty<Edge>());

            Assert.True(actual.IsSuccessful);
        }

        [Fact]
        public void Solution_ContainsWalkedEdges()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var d = new Vertex("d");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);
            var cd = new Edge("cd", c, d);

            var actual = new Solution(ab, bc);

            Assert.Contains(ab, actual.Edges);
            Assert.Contains(bc, actual.Edges);
            Assert.DoesNotContain(cd, actual.Edges);
        }

        [Fact]
        public void Solution_ContainsVisitedVertices()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var d = new Vertex("d");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);
            var cd = new Edge("cd", c, d);

            var actual = new Solution(ab, bc);

            Assert.Contains(a, actual.Vertices);
            Assert.Contains(b, actual.Vertices);
            Assert.Contains(c, actual.Vertices);
            Assert.DoesNotContain(d, actual.Vertices);
        }

        [Fact]
        public void Solution_CalculatesCorrrectWeight()
        {
            var a = new Vertex("a") { Weight = 0x1 };
            var b = new Vertex("b") { Weight = 0x2 };
            var c = new Vertex("c") { Weight = 0x4 };
            var d = new Vertex("d") { Weight = 0x8 };
            var ab = new Edge("ab", a, b) { Weight = 0x10 };
            var bc = new Edge("bc", b, c) { Weight = 0x20 };
            var cd = new Edge("cd", c, d) { Weight = 0x40 };

            var actual = new Solution(ab, bc);

            Assert.Equal(0x37, actual.Weight);
        }
    }
}
