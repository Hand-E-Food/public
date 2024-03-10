using PsiFi.Models;
using PsiFi.View;

namespace PsiFi.StateMachine
{
    class ExitMap : IStateMachineNode
    {
        private readonly GenerateMap GenerateMap = null!;

        private readonly MapView mapView;

        public ExitMap(MapView mapView)
        {
            this.mapView = mapView;
        }

        public IStateMachineNode? Execute(State state)
        {
            mapView.MoveCursorToEnd();
            mapView.Uninitialise();
            if (state.Player.Cell != null)
            {
                state.Player.Cell.Occupant = null;
                state.Player.Cell = null;
            }
            state.Map = null!;
            return GenerateMap;
        }
    }
}
