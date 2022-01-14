using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphSolver
{
    /// <summary>
    /// A solution detailing the edges and vertices traversed.
    /// </summary>
    /// <remarks><see cref="Edge.IsBidirectional"/> is ignored.</remarks>
    public class Solution : IEquatable<Solution>
    {
        /// <summary>
        /// A successful solution containing zero edges and vertices.
        /// </summary>
        public static readonly Solution Empty = new(Enumerable.Empty<Edge>());

        /// <summary>
        /// An unsuccessful solution.
        /// </summary>
        public static readonly Solution Impossible = new();

        /// <summary>
        /// <see cref="Edges"/> sorted.
        /// </summary>
        /// <remarks>Used for equality comparison.</remarks>
        private readonly Edge[] edgesSorted;

        /// <summary>
        /// This object's hash code.
        /// </summary>
        /// <remarks>Used for equality comparison.</remarks>
        private readonly int hashCode;

        /// <summary>
        /// The edges walked in this solution.
        /// </summary>
        public IReadOnlyList<Edge> Edges { get; }

        /// <summary>
        /// True if this solution is successful.
        /// False if no solution could be found.
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// The vertices visited in this solution.
        /// </summary>
        public IReadOnlyList<Vertex> Vertices { get; }

        /// <summary>
        /// This solution's total weight.
        /// </summary>
        public int Weight { get; } = 0;

        /// <summary>
        /// Initialises an unsuccessful <see cref="Solution"/>.
        /// </summary>
        private Solution()
        {
            Edges = Array.Empty<Edge>();
            edgesSorted = Array.Empty<Edge>();
            hashCode = 0;
            IsSuccessful = false;
            Vertices = Array.Empty<Vertex>();
            Weight = 0;
        }

        /// <summary>
        /// Initialises a successful <see cref="Solution"/>.
        /// </summary>
        /// <param name="edges">The edges walked.</param>
        public Solution(params Edge[] edges) : this((IEnumerable<Edge>)edges)
        { }

        /// <summary>
        /// Initialises a successful <see cref="Solution"/>.
        /// </summary>
        /// <param name="edges">The edges walked.</param>
        public Solution(IEnumerable<Edge> edges)
        {
            Edges = edges.Distinct().ToArray();
            edgesSorted = Edges.OrderBy(edge => edge.GetHashCode()).ToArray();
            IsSuccessful = true;
            Vertices = edges.SelectMany(edge => edge.Vertices).Distinct().ToArray();
            Weight = Edges.Sum(edge => edge.Weight) + Vertices.Sum(vertex => vertex.Weight);

            var hashCodeBuilder = new HashCode();
            hashCodeBuilder.Add(true);
            foreach (var edge in edgesSorted)
                hashCodeBuilder.Add(edge);
            hashCode = hashCodeBuilder.ToHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj?.GetType() == typeof(Solution) && Equals((Solution)obj);

        /// <inheritdoc/>
        public bool Equals(Solution other)
        {
            return Weight == other.Weight
                && Edges.Count == other.Edges.Count
                && edgesSorted.SequenceEqual(other.edgesSorted);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => hashCode;
    }
}
