using PsiFi.Rules;
using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Models
{
    /// <summary>
    /// A moving, acting occupant.
    /// </summary>
    abstract class Mob : Occupant, IActor
    {
        /// <inheritdoc/>
        public int ActTime
        {
            get => actTime;
            set
            {
                if (actTime == value) return;
                actTime = value;
                ActTimeChanged?.Invoke(this);
            }
        }
        private int actTime = 0;
        /// <inheritdoc/>
        public event ActorEventHandler? ActTimeChanged;

        /// <inheritdoc/>
        public override bool BlocksEnergy => false;

        /// <inheritdoc/>
        public override bool BlocksProjectiles => false;

        /// <inheritdoc/>
        public abstract IBrain? Brain { get; }

        /// <summary>
        /// This mob's cell.
        /// </summary>
        public Cell? Cell 
        {
            get => cell;
            set
            {
                if (cell == value) return;
                
                var oldCell = cell;
                cell = null;
                if (oldCell != null)
                    oldCell.Occupant = null;

                cell = value;
                if (cell != null)
                    cell.Occupant = this;
            }
        }
        private Cell? cell;

        /// <inheritdoc/>
        public override bool IsStructure => false;

        /// <inheritdoc/>
        /// <summary>Initialises a new instance of the <see cref="Mob"/> class.</summary>
        public Mob(Appearance appearance) : base(appearance)
        { }

        /// <inheritdoc/>
        public virtual IEnumerable<IActor> GetChildActors() => Enumerable.Empty<IActor>();

        /// <summary>
        /// This mob's melee attack effects.
        /// </summary>
        public virtual IEffect[] MeleeEffects { get; protected set; } = new IEffect[0];

        /// <summary>
        /// Gets this mob's visions.
        /// </summary>
        public virtual IEnumerable<Vision> GetVisions() => new[] { new Vision(Cell!.Location, Vision.Optical, 7) };

        /// <summary>
        /// The time taken to launch a melee attack.
        /// </summary>
        public virtual int MeleeDuration { get; protected set; } = 10;

        /// <summary>
        /// The time taken to walk to an adjacent cell.
        /// </summary>
        public virtual int WalkDuration { get; protected set; } = 10;
    }
}
