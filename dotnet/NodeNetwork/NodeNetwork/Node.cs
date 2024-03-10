using System.Collections.Generic;
using System.Drawing;

namespace NodeNetwork
{
    /// <summary>
    /// A <see cref="Node"/> on a <see cref="Map"/>.
    /// </summary>
    public class Node
    {
        internal List<Bridge> _bridges = new List<Bridge>();

        /// <summary>
        /// The <see cref="Bridge"/> objects linking this <see cref="Node"/> to other <see cref="Node"/> objetcs.
        /// </summary>
        public IEnumerable<Bridge> Bridges => _bridges;

        /// <summary>
        /// The free-text label of this <see cref="Node"/>.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The physical location of this <see cref="Node"/>.
        /// </summary>
        public PointF Location { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        public Node()
        { }
    }
}
