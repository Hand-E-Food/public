namespace GraphSolver.SmallestNetwork
{
    /// <summary>
    /// Attempts to find the smallest network that visits all required edges and vertices.
    /// </summary>
    public class Solver : SolverBase<Solver, Parameters>
    {
        /// <summary>
        /// Initialises a new <see cref="Solver"/>.
        /// </summary>
        /// <param name="graph">The graph to solve.</param>
        protected Solver(Graph graph) : base(graph)
        { }

        /// <summary>
        /// Initialises a new <see cref="Solver"/>.
        /// </summary>
        /// <param name="graph">The graph to solve.</param>
        /// <returns>The <see cref="Solver"/> object for method chaining.</returns>
        public static Solver For(Graph graph) => new(graph);
        
        /// <inheritdoc/>
        public override Solution Solve() => new Engine(Parameters).Solve();
    }
}
