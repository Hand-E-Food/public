using PsiFi.ConsoleForms;
using PsiFi.Mapping.Actions;
using Rogue;
using Rogue.Mapping;
using System;

namespace PsiFi.Mapping
{
    /// <summary>
    /// Manages player interaction during their turn.
    /// </summary>
    public class PlayerEngine
    {
        private FieldOfView<Cell> fieldOfView = null!;

        private Map map = null!;

        /// <summary>
        /// The map screen user interface.
        /// </summary>
        public MapScreen MapScreen { get; set; } = null!;

        /// <summary>
        /// The player.
        /// </summary>
        public Player Player { get; set; } = null!;

        /// <summary>
        /// Initialises this class after all properties are set.
        /// </summary>
        public void Initialize()
        {
            if (MapScreen == null) throw new ArgumentNullException(nameof(MapScreen));
            if (Player == null) throw new ArgumentNullException(nameof(Player));

            map = MapScreen.Map;
            fieldOfView = new FieldOfView<Cell>(map);
        }

        /// <summary>
        /// Interacts with the user to get the player's next action.
        /// </summary>
        /// <returns>The player's action.</returns>
        public IAction Act()
        {
            CalculateCellVisibility();
            MapScreen.InvalidateMapView();
            while (true)
            {
                var key = MapScreen.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.NumPad1: return Interact(Vector.SW);
                    case ConsoleKey.NumPad2: return Interact(Vector.S );
                    case ConsoleKey.NumPad3: return Interact(Vector.SE);
                    case ConsoleKey.NumPad4: return Interact(Vector.W );
                    case ConsoleKey.NumPad6: return Interact(Vector.E );
                    case ConsoleKey.NumPad7: return Interact(Vector.NW);
                    case ConsoleKey.NumPad8: return Interact(Vector.N );
                    case ConsoleKey.NumPad9: return Interact(Vector.NE);
                }
            }
        }

        private void CalculateCellVisibility()
        {
            foreach (var cell in map.AllCells)
                cell.IsVisible = false;

            if (Player.Cell != null)
                foreach (var cell in fieldOfView.ComputeFov(Player.Cell.Location, 7, true, cell => true || cell.IsTransparent))
                    cell.IsVisible = true;
        }

        private IAction Interact(Vector direction)
        {
            var targetCell = map[Player.Cell!.Location + direction];
            if (!targetCell.IsWalkable)
            {
                Player.UseTime(1000);
                return new NoAction("You walk into a wall.");
            }
            else
            {
                Player.UseTime(1000);
                return new WalkAction(Player, targetCell);
            }
        }
    }
}
