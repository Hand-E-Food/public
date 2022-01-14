using System.Linq;
using Xunit;

namespace GraphSolver
{
    public class SolverBase_Tests
    {
        /// <summary>
        /// Creates a simple graph of two triangles with a shared side.
        /// </summary>
        /// <returns>A graph.</returns>
        private static Graph CreateGraph()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var d = new Vertex("d");
            var ab = new Edge("ab", a, b);
            var ac = new Edge("ac", a, c);
            var bc = new Edge("bc", b, c);
            var bd = new Edge("bd", b, d);
            var cd = new Edge("cd", c, d);
            return new Graph(ab, ac, bc, bd, cd);
        }

        [Fact]
        public void Constructor_PopulatesParameters()
        {
            var graph = CreateGraph();
            var solver = new MockSolver(graph);
            Assert.Same(graph, solver.Parameters.Graph);
        }

        [Fact]
        public void RequiringEdge_DeclaresEdgeAsRequired()
        {
            var graph = CreateGraph();
            var edge = graph.Edges.Values.First();
            var solver = new MockSolver(graph).Requiring(edge);
            Assert.Contains(edge, solver.Parameters.RequiredEdges);
        }

        [Fact]
        public void RequiringEdge_DeclaresEdgeVerticesAsRequired()
        {
            var graph = CreateGraph();
            var edge = graph.Edges.Values.First();
            var solver = new MockSolver(graph).Requiring(edge);
            foreach (var vertex in edge.Vertices)
                Assert.Contains(vertex, solver.Parameters.RequiredVertices);
        }

        [Fact]
        public void RequiringEdges_DeclaresEdgesAsRequired()
        {
            var graph = CreateGraph();
            var solver = new MockSolver(graph).Requiring(new[] { graph.Edges["bc"], graph.Edges["bd"] });
            Assert.DoesNotContain(graph.Edges["ab"], solver.Parameters.RequiredEdges);
            Assert.DoesNotContain(graph.Edges["ac"], solver.Parameters.RequiredEdges);
            Assert.Contains(graph.Edges["bc"], solver.Parameters.RequiredEdges);
            Assert.Contains(graph.Edges["bd"], solver.Parameters.RequiredEdges);
            Assert.DoesNotContain(graph.Edges["cd"], solver.Parameters.RequiredEdges);
        }

        [Fact]
        public void RequiringEdges_DeclaresEdgesVerticesAsRequired()
        {
            var graph = CreateGraph();
            var solver = new MockSolver(graph).Requiring(new[] { graph.Edges["bc"], graph.Edges["bd"] });
            Assert.DoesNotContain(graph.Vertices["a"], solver.Parameters.RequiredVertices);
            Assert.Contains(graph.Vertices["b"], solver.Parameters.RequiredVertices);
            Assert.Contains(graph.Vertices["c"], solver.Parameters.RequiredVertices);
            Assert.Contains(graph.Vertices["d"], solver.Parameters.RequiredVertices);
        }

        [Fact]
        public void RequiringAllVerticies_DeclaresAllVerticesAsRequired()
        {
            var graph = CreateGraph();
            var solver = new MockSolver(graph).RequiringAllVertices();
            foreach (var vertex in graph.Vertices.Values)
                Assert.Contains(vertex, solver.Parameters.RequiredVertices);
        }

        private class MockSolver : SolverBase<MockSolver, Parameters>
        {
            public new Parameters Parameters => base.Parameters;

            public MockSolver(Graph graph) : base(graph)
            { }

            public override Solution Solve() => Solution.Impossible;
        }
    }
}
