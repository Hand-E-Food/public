using System.Linq;
using Xunit;

namespace GraphSolver.SmallestNetwork
{
    public class Engine_Solve_Tests : EngineBase_Solve_Tests
    {
        [Fact(DisplayName = "Requiring all vertices of an equalateral quadrilateral: solution is any 3 edges")]
        public void EquilateralQuadrilateral()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var d = new Vertex("d");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);
            var cd = new Edge("cd", c, d);
            var da = new Edge("da", d, a);
            var graph = new Graph(ab, bc, cd, da);
            var solver = Solver.For(graph)
                .RequiringAllVertices();
            var solution = solver.Solve();

            AssertRequirements(solver.Parameters, solution);
            Assert.Equal(3, solution.Edges.Count);
            Assert.Equal(3, solution.Edges.Distinct().Count());
        }

        [Fact(DisplayName = "Requiring all vertices of an equilateral triangle: solution is any 2 edges")]
        public void EquilateralTriangle()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);
            var ca = new Edge("ca", c, a);
            var graph = new Graph(ab, bc, ca);
            var solver = Solver.For(graph).RequiringAllVertices();
            var solution = solver.Solve();

            AssertRequirements(solver.Parameters, solution);
            Assert.Equal(2, solution.Edges.Count);
            Assert.Equal(2, solution.Edges.Distinct().Count());
        }

        [Fact(DisplayName = "Requiring 2 vertices where no path exists between them: solution is impossible")]
        public void NoPathBetweenRequiredVertices()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var d = new Vertex("d");
            var ab = new Edge("ab", a, b);
            var cd = new Edge("cd", c, d);
            var graph = new Graph(ab, cd);
            var solver = Solver.For(graph).Requiring(a, d);
            var solution = solver.Solve();

            Assert.False(solution.IsSuccessful);
        }

        [Fact(DisplayName = "Requiring 0 vertices or edges: solution is empty")]
        public void NoRequiredEdgesOrVertices()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var ab = new Edge("ab", a, b);
            var graph = new Graph(ab);
            var solver = Solver.For(graph);
            var solution = solver.Solve();
            var expected = Solution.Empty;

            AssertRequirements(solver.Parameters, solution);
            Assert.Equal(expected, solution);
        }
    }
}
