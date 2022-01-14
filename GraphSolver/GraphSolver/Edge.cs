using System;
using System.Collections.Generic;

namespace GraphSolver
{
    /// <summary>
    /// A graph's edge between two vertices.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// The vertex at the end of this edge.
        /// </summary>
        public Vertex EndVertex => Vertices[1];

        /// <summary>
        /// This edge's id.
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// True if this edge can be walked in either direction.
        /// False if this edge can only be walked from start to end.
        /// Default is true.
        /// </summary>
        public bool IsBidirectional { get; set; } = DefaultIsBidirectional;
        /// <summary>
        /// The default value of <see cref="IsBidirectional"/>.
        /// </summary>
        internal const bool DefaultIsBidirectional = true;

        /// <summary>
        /// The vertex at the start of this edge.
        /// </summary>
        public Vertex StartVertex => Vertices[0];

        /// <summary>
        /// The vertices at the start and end of this edge.
        /// </summary>
        /// <remarks>
        /// The first element is this edge's starting vertex.
        /// The second element is this edge's ending vertex.
        /// </remarks>
        public IReadOnlyList<Vertex> Vertices { get; }

        /// <summary>
        /// The weight added to the score when this edge is added to the solution.
        /// Default is 1.
        /// </summary>
        public int Weight { get; set; } = DefaultWeight;
        /// <summary>
        /// The default value of <see cref="Weight"/>.
        /// </summary>
        internal const int DefaultWeight = 1;

        /// <summary>
        /// Initialises a new edge.
        /// </summary>
        /// <param name="id">This edge's id.</param>
        /// <param name="startVertex">The vertex at the start of this edge.</param>
        /// <param name="endVertex">The vertex at the end of this edge.</param>
        /// <exception cref="ArgumentNullException"><paramref name="startVertex"/> or <paramref name="endVertex"/> is null.</exception>
        public Edge(object id, Vertex startVertex, Vertex endVertex)
        {
            if (startVertex == null) throw new ArgumentNullException(nameof(startVertex));
            if (endVertex == null) throw new ArgumentNullException(nameof(endVertex));
            Id = id;
            Vertices = new[] { startVertex, endVertex };
            foreach(var vertex in Vertices)
                vertex.AddEdge(this);
        }

        /// <summary>
        /// Determines if this edge can be walked from the specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to walk from.</param>
        /// <returns>
        /// True if this edge is bidirectional, or this edge is unidirectional and the specified vertex is the start vertex.
        /// Otherwise, false.
        /// </returns>
        public bool CanWalkFrom(Vertex vertex) => vertex == StartVertex || (IsBidirectional && vertex == EndVertex);

        /// <summary>
        /// Gets the vertex at the other end of this edge from the specified vertex.
        /// </summary>
        /// <param name="vertex">The vertex to start from.</param>
        /// <returns>The vertex at the other end of this edge.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="vertex"/> is null.</exception>
        /// <exception cref="ArgumentException">The specified vertex is not joined to this edge.</exception>
        public Vertex GetOtherVertex(Vertex vertex)
        {
            if (StartVertex == vertex)
                return EndVertex;
            else if (EndVertex == vertex)
                return StartVertex;
            else if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            else
                throw new ArgumentException("The specified vertex is not joined to this edge.", nameof(vertex));
        }
    }
}
