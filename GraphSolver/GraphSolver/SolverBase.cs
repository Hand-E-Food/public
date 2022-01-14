using System.Collections.Generic;

namespace GraphSolver
{
    /// <summary>
    /// Attempts to solve a graph, visiting all required edges and vertices.
    /// </summary>
    /// <typeparam name="TSolver">The subclass that is extending this class.</typeparam>
    /// <typeparam name="TParameters">The class that defines this solver's parameters.</typeparam>
    public abstract class SolverBase<TSolver, TParameters>
        where TSolver : SolverBase<TSolver, TParameters>
        where TParameters : Parameters, new()
    {
        /// <summary>
        /// The parameters to solve for.
        /// </summary>
        public TParameters Parameters { get; }

        /// <summary>
        /// Returns this object type cast as a <see cref="TSolver"/>.
        /// </summary>
        protected TSolver This => (TSolver)this;

        /// <summary>
        /// Initialises a new <see cref="SolverBase{TSolver, TParameters}"/>.
        /// </summary>
        /// <param name="graph">The graph to solve.</param>
        protected SolverBase(Graph graph)
        {
            Parameters = new TParameters { Graph = graph };
        }

        /// <summary>
        /// Declares an edge that must be included in the solution.
        /// </summary>
        /// <param name="edge">The edge that must be included in the solution.</param>
        /// <returns>This object for method chaining.</returns>
        public TSolver Requiring(Edge edge)
        {
            Parameters.RequiredEdges.Add(edge);
            foreach (var vertex in edge.Vertices)
                Parameters.RequiredVertices.Add(vertex);
            return This;
        }

        /// <summary>
        /// Declares edges that must be included in the solution.
        /// </summary>
        /// <param name="edges">The edges that must be included in the solution.</param>
        /// <returns>This object for method chaining.</returns>
        public TSolver Requiring(params Edge[] edges) => Requiring((IEnumerable<Edge>)edges);

        /// <summary>
        /// Declares edges that must be included in the solution.
        /// </summary>
        /// <param name="edges">The edges that must be included in the solution.</param>
        /// <returns>This object for method chaining.</returns>
        public TSolver Requiring(IEnumerable<Edge> edges)
        {
            foreach (var edge in edges)
                Requiring(edge);
            return This;
        }

        /// <summary>
        /// Declares a vertex that must be included in the solution.
        /// </summary>
        /// <param name="vertex">The vertex that must be included in the solution.</param>
        /// <returns>This object for method chaining.</returns>
        public TSolver Requiring(Vertex vertex)
        {
            Parameters.RequiredVertices.Add(vertex);
            return This;
        }

        /// <summary>
        /// Declares vertices that must be included in the solution.
        /// </summary>
        /// <param name="vertices">The vertices that must be included in the solution.</param>
        /// <returns>This object for method chaining.</returns>
        public TSolver Requiring(params Vertex[] vertices) => Requiring((IEnumerable<Vertex>)vertices);

        /// <summary>
        /// Declares vertices that must be included in the solution.
        /// </summary>
        /// <param name="vertices">The vertices that must be included in the solution.</param>
        /// <returns>This object for method chaining.</returns>
        public TSolver Requiring(IEnumerable<Vertex> vertices)
        {
            foreach (var vertex in vertices)
                Parameters.RequiredVertices.Add(vertex);
            return This;
        }

        /// <summary>
        /// Declares that all vertices in the graph must be included in the solution.
        /// </summary>
        /// <returns>This object for method chaining.</returns>
        public TSolver RequiringAllVertices() => Requiring(Parameters.Graph.Vertices.Values);

        /// <summary>
        /// Attempts to solve the graph with the declared parameters.
        /// </summary>
        /// <returns>The solution.</returns>
        public abstract Solution Solve();
    }
}
