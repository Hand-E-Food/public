namespace GraphSolver.SmallestNetwork
{
    /// <summary>
    /// Performs the actual work to solve the graph.
    /// </summary>
    public class Engine
    {
        /// <summary>
        /// The parameters to solve for.
        /// </summary>
        private readonly Parameters parameters;

        /// <summary>
        /// Initialises a new <see cref="Engine"/>.
        /// </summary>
        /// <param name="solver">The parameters to solve for.</param>
        public Engine(Parameters parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// Finds the smallest network that visits all required edges and vertices.
        /// </summary>
        /// <returns>The solution.</returns>
        public Solution Solve()
        {
            if (parameters.RequiredEdges.Count == 0 && parameters.RequiredVertices.Count == 0)
                return Solution.Empty;

            //TODO

            return Solution.Impossible;
        }
    }
}
