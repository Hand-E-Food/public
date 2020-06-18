using PsiFi.Engines;

namespace PsiFi.Models.Mapping
{
    interface IActor
    {

        /// <summary>
        /// The time index when this actor can next act.
        /// </summary>
        int NextTimeIndex { get; set; }

        /// <summary>
        /// Perform this actor's next action.
        /// </summary>
        /// <param name="mapInterface">The interface through which this actor can view and interact with the map.</param>
        void Interact(MapInterface mapInterface);
    }
}
