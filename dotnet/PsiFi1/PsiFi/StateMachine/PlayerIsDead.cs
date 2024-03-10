using PsiFi.Models;
using PsiFi.View;
using System;

namespace PsiFi.StateMachine
{
    class PlayerIsDead : IStateMachineNode
    {
        private readonly MapView mapView;

        public PlayerIsDead(MapView mapView)
        {
            this.mapView = mapView;
        }

        public IStateMachineNode? Execute(State state)
        {
            state.Player.Appearance = new Appearance('@', ConsoleColor.Red, ConsoleColor.DarkRed);
            mapView.MoveCursorToEnd();
            return null;
        }
    }
}