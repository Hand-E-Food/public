using System.Linq;
using Xunit;

namespace GraphSolver
{
    public abstract class EngineBase_Solve_Tests
    {
        /// <summary>
        /// Asserts that the <paramref name="solution"/> meets the requirements declared in the
        /// <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters">The parameters declaring the requirements.</param>
        /// <param name="solution">The solution.</param>
        public void AssertRequirements(Parameters parameters, Solution solution)
        {
            Assert.True(solution.IsSuccessful);
            Assert.Superset(parameters.RequiredEdges, solution.Edges.ToHashSet());
            Assert.Superset(parameters.RequiredVertices, solution.Vertices.ToHashSet());
        }
    }
}
