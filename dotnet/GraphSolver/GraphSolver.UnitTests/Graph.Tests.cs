using Xunit;

namespace GraphSolver
{
    public class Graph_Tests
    {
        [Fact]
        public void Constructor_WithEdges_AddsAllEdges()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);

            var graph = new Graph(ab, bc);

            Assert.Contains(ab, graph.Edges.Values);
            Assert.Contains(bc, graph.Edges.Values);
        }

        [Fact]
        public void Constructor_WithEdges_AddsAllVertices()
        {
            var a = new Vertex("a");
            var b = new Vertex("b");
            var c = new Vertex("c");
            var ab = new Edge("ab", a, b);
            var bc = new Edge("bc", b, c);

            var graph = new Graph(ab, bc);

            Assert.Contains(a, graph.Vertices.Values);
            Assert.Contains(b, graph.Vertices.Values);
            Assert.Contains(c, graph.Vertices.Values);
        }

        [Fact]
        public void Constructor_WithObjects_AddsAllEdges()
        {
            var graph = Graph.From(
                new[] { "ab", "bc" },
                startVertexIdSelector: edge => edge[0],
                endVertexIdSelector: edge => edge[1]
            );

            Assert.Contains(graph.Edges.Values, edge => edge.Id.Equals("ab"));
            Assert.Contains(graph.Edges.Values, edge => edge.Id.Equals("bc"));
        }

        [Fact]
        public void Constructor_WithObjects_AddsAllVertices()
        {
            var graph = Graph.From(
                new[] { "ab", "bc" },
                startVertexIdSelector: edge => edge[0],
                endVertexIdSelector: edge => edge[1]
            );

            Assert.Contains(graph.Vertices.Values, vertex => vertex.Id.Equals('a'));
            Assert.Contains(graph.Vertices.Values, vertex => vertex.Id.Equals('b'));
            Assert.Contains(graph.Vertices.Values, vertex => vertex.Id.Equals('c'));
        }

        [Fact]
        public void Constructor_WithObjects_SetsEdgeWeight()
        {
            var graph = Graph.From(
                new[] { "ab", "bc" },
                startVertexIdSelector: edge => edge[0],
                endVertexIdSelector: edge => edge[1],
                edgeWeightSelector: edge => edge[1] - 'a'
            );

            Assert.Contains(graph.Edges.Values, edge => edge.Id.Equals("ab") && edge.Weight == 1);
            Assert.Contains(graph.Edges.Values, edge => edge.Id.Equals("bc") && edge.Weight == 2);
        }

        [Fact]
        public void Constructor_WithObjects_SetsVertexWeigth()
        {
            var graph = Graph.From(
                new[] { "ab", "bc" },
                startVertexIdSelector: edge => edge[0],
                endVertexIdSelector: edge => edge[1],
                vertexWeightSelector: vertex => vertex - 'a'
            );

            Assert.Contains(graph.Vertices.Values, vertex => vertex.Id.Equals('a') && vertex.Weight == 0);
            Assert.Contains(graph.Vertices.Values, vertex => vertex.Id.Equals('b') && vertex.Weight == 1);
            Assert.Contains(graph.Vertices.Values, vertex => vertex.Id.Equals('c') && vertex.Weight == 2);
        }

        [Fact]
        public void Constructor_WithObjects_SetsEdgeIsBidirectional()
        {
            var graph = Graph.From(
                new[] { "ab", "bc" },
                startVertexIdSelector: edge => edge[0],
                endVertexIdSelector: edge => edge[1],
                edgeIsBidirectionalSelector: edge => edge[1] == 'b'
            );

            Assert.Contains(graph.Edges.Values, edge => edge.Id.Equals("ab") && edge.IsBidirectional == true);
            Assert.Contains(graph.Edges.Values, edge => edge.Id.Equals("bc") && edge.IsBidirectional == false);
        }
    }
}
