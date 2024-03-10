using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NodeNetwork
{
    /// <summary>
    /// A <see cref="Bridge"/> linking two <see cref="Node"/> objects.
    /// </summary>
    public class Bridge
    {
        internal Node[] _nodes;

        /// <summary>
        /// The free-text label of this <see cref="Bridge"/>.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The <see cref="Node"/> objects linked by this <see cref="Bridge"/>.
        /// </summary>
        public IEnumerable<Node> Nodes => _nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bridge"/> class.
        /// </summary>
        public Bridge()
        {
            _nodes = new Node[2];
        }

        internal Bridge(IEnumerable<Node> nodes)
        {
            _nodes = nodes.ToArray();
            foreach (Node node in _nodes)
                node._bridges.Add(this);
        }

        /// <summary>
        /// Sets which <see cref="Node"/> objects are linked by this <see cref="Bridge"/>.
        /// </summary>
        /// <param name="node1">The first <see cref="Node"/>.</param>
        /// <param name="node2">The second <see cref="Node"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="node1"/> or <paramref name="node2"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="node1"/> and <paramref name="node2"/> are the same.</exception>
        public void LinkNodes(Node node1, Node node2)
        {
            if (node1 == null)
                throw new ArgumentNullException(nameof(node1));
            if (node2 == null)
                throw new ArgumentNullException(nameof(node2));
            if (node1 == node2)
                throw new ArgumentException($"{nameof(node1)} cannot be the same as {nameof(node2)}.");

            foreach (var node in _nodes.Where(x => x != null))
                node._bridges.Remove(this);

            _nodes[0] = node1;
            _nodes[1] = node2;

            foreach (Node node in _nodes)
                node._bridges.Add(this);
        }

        /// <summary>
        /// Removes the link between the <see cref="Nodes"/>.
        /// </summary>
        public void UnlinkNodes()
        {
            foreach (var node in _nodes.Where(x => x != null))
                node._bridges.Remove(this);

            _nodes[0] = null;
            _nodes[1] = null;
        }
    }
}
