using System.Collections.Generic;
using System.Linq;

namespace GraphSolver
{
    /// <summary>
    /// A graph's vertex.
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// The edges extending from this vertex.
        /// </summary>
        public IReadOnlyCollection<Edge> Edges => edges;
        private readonly List<Edge> edges = new();

        /// <summary>
        /// This vertex's id.
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// Gets the edges that can be walked from this vertex.
        /// </summary>
        public IEnumerable<Edge> WalkableEdges => edges.Where(edge => edge.IsBidirectional || edge.StartVertex == this);

        /// <summary>
        /// The weight added to the score when this vertex is added to the solution.
        /// Default is 0.
        /// </summary>
        public int Weight { get; set; } = DefaultWeight;
        /// <summary>
        /// The default value of <see cref="Weight"/>.
        /// </summary>
        internal const int DefaultWeight = 0;

        /// <summary>
        /// Initialises a new vertex.
        /// </summary>
        /// <param name="id">The vertex's id.</param>
        public Vertex(object id)
        {
            Id = id;
        }

        /// <summary>
        /// Adds an edge to this vertex.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        internal void AddEdge(Edge edge)
        {
            if (!edges.Contains(edge)) edges.Add(edge);
        }
    }
}
