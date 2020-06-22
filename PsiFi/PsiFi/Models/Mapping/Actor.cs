using PsiFi.Engines;
using System;

namespace PsiFi.Models.Mapping
{
    /// <summary>
    /// An object that can act upon a map.
    /// </summary>
    abstract class Actor
    {

        /// <summary>
        /// The time index when this actor can next act.
        /// </summary>
        public int NextTimeIndex 
        { 
            get => nextTimeIndex; 
            set
            {
                if (nextTimeIndex == value) return;
                var e = new ValueChangedEventArgs<int>(nextTimeIndex, value);
                nextTimeIndex = value;
                NextTimeIndexChanged?.Invoke(this, e);
            }
        }
        private int nextTimeIndex;

        /// <summary>
        /// Raised when the value of <see cref="NextTimeIndex"/> changes.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<int>> NextTimeIndexChanged;

        /// <summary>
        /// Perform this actor's next action.
        /// </summary>
        /// <param name="mapInterface">The interface through which this actor can view and interact with the map.</param>
        public abstract void Interact(MapInterface mapInterface);

        /// <summary>
        /// Changes <see cref="NextTimeIndex"/> for the specified action.
        /// </summary>
        /// <param name="action">The action being performed.</param>
        /// <remarks>By default, any action takes 1000ms.</remarks>
        public virtual void SetNextTimeIndexFor(object action) => NextTimeIndex += 1000;
    }
}
