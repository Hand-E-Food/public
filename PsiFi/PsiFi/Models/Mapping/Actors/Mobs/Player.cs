using PsiFi.Engines;
using PsiFi.Views;
using System;

namespace PsiFi.Models.Mapping.Actors.Mobs
{
    class Player : Mob
    {
        public IMapView MapView { get; set; }

        public Player() : base(new Appearance('@', ConsoleColor.White), 100)
        { }

        public override void Interact(MapInterface mapInterface)
        {
            var response = MapView.GetPlayerInput();
            if (Perform(response, mapInterface))
                SetNextTimeIndexFor(response.Action);
        }

        private bool Perform(MapViewResponse response, MapInterface mapInterface)
        {
            switch (response.Action)
            {
                case MapViewAction.Quit: return false;
                case MapViewAction.Wait: return true;
                case MapViewAction.Move: return mapInterface.Move(response.Direction);
                default: return false;
            }
        }
    }
}
