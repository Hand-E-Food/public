using PsiFi.Geometry;
using PsiFi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Rules
{
    /// <summary>
    /// A targetted attack.
    /// </summary>
    partial class Attack
    {
        private static readonly IEnumerable<IEffect> EmptyEffects = Enumerable.Empty<IEffect>();
        private static readonly IList<Point> EmptyPath = new Point[0];

        /// <summary>
        /// This attack's chance of hitting its target in percentage points.
        /// </summary>
        public int? Accuracy { get; protected set; }

        /// <summary>
        /// This attack's appearance.
        /// </summary>
        public Appearance? Appearance { get; protected set; }

        /// <summary>
        /// The effects of this attack if it hits the target.
        /// </summary>
        protected IEnumerable<IEffect> HitEffects { get; set; } = EmptyEffects;

        /// <summary>
        /// The effects of this attack if it misses the target.
        /// </summary>
        protected IEnumerable<IEffect> MissEffects { get; set; } = EmptyEffects;

        /// <summary>
        /// The <see cref="Mob"/> that launched this attack.
        /// </summary>
        public Mob Origin { get; protected set; } = null!;

        /// <summary>
        /// The points this attack passes through, if any.
        /// </summary>
        public IList<Point> Path { get; protected set; } = EmptyPath;

        /// <summary>
        /// This cell's target.
        /// </summary>
        public Cell Target { get; protected set; } = null!;

        /// <summary>
        /// Initialises a new instance of the <see cref="Attack"/> class.
        /// </summary>
        protected Attack()
        { }

        /// <summary>
        /// Adjusts this attacks accuracy by <paramref name="delta"/> percentage points.
        /// </summary>
        /// <param name="delta">The number of percentage points by which to adjust this attacks accuracy.</param>
        public void AdjustAccuracy(int delta)
        {
            if (Accuracy.HasValue)
                Accuracy += delta;
        }

        /// <summary>
        /// Launches the attack.
        /// </summary>
        /// <returns>The attack's effect.</returns>
        public virtual IEnumerable<IEffect> Execute(State state)
        {
            return Accuracy == null || state.Random.Next(100) < Accuracy.Value
                ? HitEffects
                : MissEffects;
        }
    }
}
