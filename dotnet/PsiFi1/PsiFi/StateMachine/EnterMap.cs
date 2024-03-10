using PsiFi.Models;
using PsiFi.View;

namespace PsiFi.StateMachine
{
    class EnterMap : IStateMachineNode
    {
        private readonly SelectNextActor SelectNextActor = null!;
        
        private readonly MapView mapView;

        public EnterMap(MapView mapView)
        {
            this.mapView = mapView;
        }

        /// <inheritdoc/>
        public IStateMachineNode? Execute(State state)
        {
            mapView.Initialise(state);
            return SelectNextActor;
        }
    }
}
