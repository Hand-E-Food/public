using PsiFi.Engines;
using PsiFi.Models.Mapping.Geometry;
using PsiFi.Models.Mapping.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Models.Mapping.Actors.Mobs
{
    class MaintenanceDrone : Mob
    {
        public override Appearance Appearance { get; } = new Appearance('r', ConsoleColor.White);

        public override RangeValue Health { get; } = new RangeValue(1);

        public override List<Item> Inventory { get; } = new List<Item>(0);

        public override string Name { get; } = "Maintenance Drone";

        public override WieldedItems Slots { get; } = WieldedItems.None();

        public override void Interact(MapInterface mapInterface)
        {
            var possibleActions = GetPossibleActions(mapInterface).ToList();
            if (possibleActions.Any())
                mapInterface.Random.Next(possibleActions)();
            NextTimeIndex += 3000;
        }

        private IEnumerable<Action> GetPossibleActions(MapInterface mapInterface)
        {
            foreach (var direction in Direction.All)
            {
                var targetCell = mapInterface.Map[Cell.Location + direction];
                if (targetCell.Terrain.AllowsMovement && targetCell.Mob == null)
                    yield return () => mapInterface.Move(direction);
            }
        }
    }
}
