using System;
using System.Collections.Generic;
using System.Linq;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Navigation
{

    /// <summary>
    /// A candidate route used by navigation.
    /// </summary>
    public class Route : IComparable<Route>
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="origin">The origin junction of this route.</param>
        /// <param name="roads">The roads traversed by the route.</param>
        public Route(Junction origin)
        {
            this.Children = new List<Route>();
            this.Parent = null;
            this.Roads = new Road[] { };
            InitializeJunctions(origin);
        }
        /// <summary>
        /// Initialises a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="origin">The origin junction of this route.</param>
        /// <param name="parent">The parent route to this route.</param>
        /// <param name="road">The road to extend along.</param>
        public Route(Junction origin, Route parent, Road road)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (road == null) throw new ArgumentNullException("road");
            this.Children = new List<Route>();
            this.Parent = parent;
            this.Roads = Parent.Roads.Concat(new [] {road}).ToArray();
            InitializeJunctions(origin);
        }

        #region Properties ...

        /// <summary>
        /// Gets this route's child routes.
        /// </summary>
        /// <value>A list of routes extended from this route.</value>
        public List<Route> Children { get; private set; }

        /// <summary>
        /// Gets the list of junctions traversed by this route.
        /// </summary>
        /// <value>The list of junctions traversed by this route.</value>
        public Junction[] Junctions { get; private set; }

        /// <summary>
        /// Gets a string representation of the junctions traversed.
        /// </summary>
        public string JunctionString { get { return string.Join(" - ", Junctions.Select((j) => j.Name)); } }

        /// <summary>
        /// Gets this route's length.
        /// </summary>
        /// <value>This route's length.</value>
        public double Length { get { return Roads.Sum((r) => r.Length); } }

        /// <summary>
        /// Gets or sets this route's parent route.
        /// </summary>
        /// <value>This route's parent route.</value>
        public Route Parent
        {
            get { return _Parent; }
            set
            {
                // Remove this route as a child of its previous parent.
                if (_Parent != null)
                    _Parent.Children.Remove(this);
                // Assign this route's new parent.
                _Parent = value;
                // Ad this route as a child of its new parent.
                if (_Parent != null)
                    _Parent.Children.Add(this);
            }
        }
        private Route _Parent = null;

        /// <summary>
        /// Gets the list of roads traversed.
        /// </summary>
        /// <value>An array of roads.</value>
        public Road[] Roads { get; private set; }

        /// <summary>
        /// Gets this state's score as a solution candidate.  Lower scores are more likely to lead to a solution.
        /// </summary>
        /// <value>This state's score as a solution candidate.</value>
        public virtual double Score { get { return Length; } }

        #endregion

        /// <summary>
        /// Compares this Route object to another Route object.
        /// </summary>
        /// <param name="other">The Route object to compare to this one.</param>
        public int CompareTo(Route other)
        {
            return this.Score.CompareTo(other.Score);
        }

        /// <summary>
        /// Initialises the Junctions property after the Roads property is set.
        /// </summary>
        /// <param name="origin">The route's origin junction.</param>
        private void InitializeJunctions(Junction origin)
        {
            // Walk the route from the origin to the last junction.
            List<Junction> junctions = new List<Junction>(Roads.Length + 1);
            Junction junction = origin;
            junctions.Add(junction);
            foreach (var road in Roads)
            {
                if (!road.Junctions.Contains(junction)) throw new ArgumentException("Roads is not a continuous sequence of roads.");
                junction = road.Other(junction);
                junctions.Add(junction);
            }
            Junctions = junctions.ToArray();
        }

        /// <summary>
        /// Replaces the sequence of old roads with the new roads.
        /// </summary>
        /// <param name="oldRoute">The old route to replace.</param>
        /// <param name="newRoute">The new route to insert.</param>
        public void Replace(Route oldRoute, Route newRoute)
        {
            int oldCount = oldRoute.Roads.Count();
            // Validate the this route actually starts with the old roads.
            if (!this.Roads.Take(oldCount).SequenceEqual(oldRoute.Roads))
                throw new ArgumentException("This route must start with the same roads as oldRoute.");
            // Replace the roads.
            Roads = newRoute.Roads.Concat(Roads.Skip(oldCount)).ToArray();
            // Initialise the junctions.
            InitializeJunctions(newRoute.Junctions.First());
        }
    }
}
