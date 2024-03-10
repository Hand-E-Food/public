using System.Collections.Generic;
using System.Linq;

namespace NodeNetwork
{
    /// <summary>
    /// A collection of <see cref="Node"/> and <see cref="Bridge"/> objects.
    /// </summary>
    public class Map
    {

        /// <summary>
        /// The <see cref="Bridge"/> objects linking the <see cref="Node"/> objects.
        /// </summary>
        public IEnumerable<Bridge> Bridges => Nodes.SelectMany(node => node.Bridges).Distinct();

        /// <summary>
        /// The <see cref="Node"/> objects on this <see cref="Map"/>.
        /// </summary>
        public List<Node> Nodes { get; } = new List<Node>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        public Map()
        { }
    }
}
