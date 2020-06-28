using PsiFi.Engines;
using PsiFi.Views;
using System;

namespace PsiFi.Models.Mapping.Actors.Mobs
{
    class Player : Mob
    {
        public override Appearance Appearance { get; } = new Appearance('@', ConsoleColor.White);

        public override RangeValue Health { get; } = new RangeValue(10);

        public IMapView MapView { get; set; }

        public override string Name { get; } = "Player";

        public override WieldedItems Slots { get; } = WieldedItems.Humanoid();

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
                case MapViewAction.Quit: return mapInterface.Quit();
                case MapViewAction.Wait: return true;
                case MapViewAction.Move: return mapInterface.Move(response.Direction);
                default: return false;
            }
        }

        /// <summary>
        /// Changes <see cref="NextTimeIndex"/> for the specified action.
        /// </summary>
        /// <param name="action">The action being performed.</param>
        public virtual void SetNextTimeIndexFor(object action) => NextTimeIndex += 1000;
    }
}
