using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphSolver
{
    /// <summary>
    /// A graph of vertices and edges.
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// This graph's edges.
        /// </summary>
        public IReadOnlyDictionary<object, Edge> Edges => edges;
        private readonly Dictionary<object, Edge> edges = new();

        /// <summary>
        /// This graph's vertices.
        /// </summary>
        public IReadOnlyDictionary<object, Vertex> Vertices => vertices;
        private readonly Dictionary<object, Vertex> vertices = new();

        /// <summary>
        /// Initialises a new graph.
        /// </summary>
        /// <param name="edges">This graph's edges.</param>
        public Graph(params Edge[] edges) : this((IEnumerable<Edge>)edges)
        { }

        /// <summary>
        /// Initialises a new graph.
        /// </summary>
        /// <param name="edges">This graph's edges.</param>
        /// <exception cref="ArgumentNullException"><paramref name="edges"/> is null.</exception>
        public Graph(IEnumerable<Edge> edges) 
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            foreach (var edge in edges)
            {
                this.edges[edge.Id] = edge;
                foreach (var vertex in edge.Vertices)
                    vertices[vertex.Id] = vertex;
            }
        }

        /// <summary>
        /// Initialises a new graph.
        /// </summary>
        /// <typeparam name="TEdge">The type of each edge's id.</typeparam>
        /// <typeparam name="TVertex">The type of each vertex's id.</typeparam>
        /// <param name="edgeIds">The graph's edge ids.</param>
        /// <param name="startVertexIdSelector">A function that selects the start vertex from an edge id.</param>
        /// <param name="endVertexIdSelector">A function that selects the end vertex from an edge id.</param>
        /// <param name="edgeWeightSelector">
        /// A function that selects an edge's weight from an edge id.
        /// If omitted, uses the default <see cref="Edge.Weight"/> for an <see cref="Edge"/>.
        /// </param>
        /// <param name="edgeIsBidirectionalSelector">
        /// A function that selects whether an edge is bidirectional from an edge id.
        /// If omitted, uses the default <see cref="Edge.IsBidirectional"/> value for an <see cref="Edge"/>.
        /// </param>
        /// <param name="vertexWeightSelector">
        /// A function that selects a vertex's weight from a vertex id.
        /// If omitted, uses the default <see cref="Vertex.Weight"/> value for a <see cref="Vertex"/>.
        /// </param>
        /// <returns>An initialised <see cref="Graph"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="edgeIds"/>, <paramref name="startVertexIdSelector"/> or <paramref name="endVertexIdSelector"/> is null.</exception>
        public static Graph From<TEdge, TVertex>(
            IEnumerable<TEdge> edgeIds,
            Func<TEdge, TVertex> startVertexIdSelector,
            Func<TEdge, TVertex> endVertexIdSelector,
            Func<TEdge, int> edgeWeightSelector = null,
            Func<TEdge, bool> edgeIsBidirectionalSelector = null,
            Func<TVertex, int> vertexWeightSelector = null
        )
        {
            if (edgeIds == null) throw new ArgumentNullException(nameof(edgeIds));
            if (startVertexIdSelector == null) throw new ArgumentNullException(nameof(startVertexIdSelector));
            if (endVertexIdSelector == null) throw new ArgumentNullException(nameof(endVertexIdSelector));

            if (edgeWeightSelector == null)
                edgeWeightSelector = _ => Edge.DefaultWeight;
            
            if (edgeIsBidirectionalSelector == null)
                edgeIsBidirectionalSelector = _ => Edge.DefaultIsBidirectional;
            
            if (vertexWeightSelector == null)
                vertexWeightSelector = _ => Vertex.DefaultWeight;

            var vertices = edgeIds
                .SelectMany(edgeId => new[] { startVertexIdSelector(edgeId), endVertexIdSelector(edgeId) })
                .Distinct()
                .ToDictionary(
                    vertexId => vertexId,
                    vertexId => new Vertex(vertexId) {
                        Weight = vertexWeightSelector(vertexId),
                    }
                );

            var edges = edgeIds
                .Select(edgeId => new Edge(edgeId, vertices[startVertexIdSelector(edgeId)], vertices[endVertexIdSelector(edgeId)]) {
                    IsBidirectional = edgeIsBidirectionalSelector(edgeId),
                    Weight = edgeWeightSelector(edgeId),
                });

            return new Graph(edges);
        }
    }
}
