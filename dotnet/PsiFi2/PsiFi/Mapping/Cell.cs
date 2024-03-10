using Rogue;
using Rogue.Mapping;
using System;

namespace PsiFi.Mapping
{
    /// <summary>
    /// A cell on a map.
    /// </summary>
    public class Cell : ICell, IPhysical
    {
        /// <summary>
        /// This cell's back color.
        /// </summary>
        public virtual Color BackColor { get; set; } = Color.Black;

        /// <summary>
        /// This cell's character.
        /// </summary>
        public virtual char Character { get; set; } = ' ';

        /// <summary>
        /// This cell's fore color.
        /// </summary>
        public virtual Color ForeColor { get; set; } = Color.Gray;

        /// <summary>
        /// True is the player has seen this cell.
        /// False if the player has never seen this cell.
        /// </summary>
        public bool IsExplored { get; set; } = false;

        /// <summary>
        /// True if this cell is transparent.
        /// False if this cell blocks light.
        /// </summary>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// True if this cell is visible to the player.
        /// False if this cell cannot be currently seen by the player.
        /// </summary>
        /// <remarks>
        /// Setting this to true also sets <see cref="IsExplored"/> to true.
        /// </remarks>
        public bool IsVisible 
        {
            get => isVisible;
            set
            {
                isVisible = value;
                IsExplored |= value;
            }
        }
        private bool isVisible = false;

        /// <summary>
        /// True if this cell can be walked upon.
        /// False if this cell cannot be walked upon.
        /// </summary>
        public bool IsWalkable { get; set; }

        /// <inheritdoc/>
        public Point Location { get; set; }

        /// <summary>
        /// The mob in this cell, if any.
        /// </summary>
        /// <remarks>
        /// This should only be set from <see cref="Mob.Cell"/>.
        /// </remarks>
        public Mob? Mob
        {
            get => mob;
            set
            {
                if (mob == value) return;
                if (mob != null && value != null) throw new InvalidOperationException($"{nameof(Mob)} must be set to null before setting a new value");
                mob = value;
            }
        }
        private Mob? mob = null;
    }
}