using System.Collections.Generic;

namespace GraphSolver
{
    /// <summary>
    /// The parameters to solve for.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// The graph to solve.
        /// </summary>
        public Graph Graph { get; set; }

        /// <summary>
        /// The edges that must be included in the solution.
        /// </summary>
        public HashSet<Edge> RequiredEdges { get; } = new();

        /// <summary>
        /// The vertices that must be included in the solution.
        /// </summary>
        public HashSet<Vertex> RequiredVertices { get; } = new();
    }
}
