using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkRichardson.Battleship.Navy
{

    public class Battlefield
    {

        /// <summary>
        /// The number of attacks made on the battlefield.
        /// </summary>
        public int Attacks { get; private set; }

        /// <summary>
        /// The grid detailing the battlefield.
        /// </summary>
        private readonly Grid<ShipStatus> _grid = new Grid<ShipStatus>(Size);

        /// <summary>
        /// The random number generator used to place ships.
        /// </summary>
        private readonly Random Random;

        /// <summary>
        /// The length of each ship in the navy.
        /// </summary>
        public static readonly int[] ShipLengths = { 5, 4, 3, 3, 2 };

        /// <summary>
        /// The width and depth of the battlefield grid.
        /// </summary>
        public const int Size = 10;

        /// <summary>
        /// Initialises a new <see cref="Battlefield"/>.
        /// </summary>
        public Battlefield(int? seed = null)
        {
            if (seed.HasValue)
                Random = new Random(seed.Value);
            else
                Random = new Random();

            Attacks = 0;

            foreach (var length in ShipLengths)
                PlaceShip(length);

            FillSpace();
        }

        /// <summary>
        /// Places a ship in a random valid location.
        /// </summary>
        /// <param name="length">The length of the ship.</param>
        private void PlaceShip(int length)
        {
            var locations = FindLocations(length);
            var i = Random.Next(locations.Count());
            var location = locations[i];
            PlaceShip(location);
        }

        /// <summary>
        /// Finds all valid locations for a ship of given length.
        /// </summary>
        /// <param name="length">The length of the ship to place.</param>
        /// <returns>A list of all locations in which the ship can fit.</returns>
        private IList<Ship> FindLocations(int length)
        {
            var max = (Size + 1 - length);
            List<Ship> results = new List<Ship>(max * max);
            for(int y = 0; y < max; y++)
            {
                for (int x = 0; x < max; x++)
                {
                    bool horizontal = true, vertical = true;
                    for (int i = 0; i < length; i++)
                    {
                        horizontal &= _grid[x + i, y] == ShipStatus.Unspecified;
                        vertical   &= _grid[x, y + i] == ShipStatus.Unspecified;
                    }
                    if (horizontal)
                        results.Add(new Ship(x, y, length, Orientation.Horizontal));
                    if (vertical)
                        results.Add(new Ship(x, y, length, Orientation.Vertical));
                }
            }
            return results;
        }

        /// <summary>
        /// Places a ship of the specified length at the specified location.
        /// </summary>
        /// <param name="ship">The ship's location.</param>
        private void PlaceShip(Ship ship)
        {
            // Mark surrounding water as clear.
            var yMin = Math.Max(0, ship.Y - 1);
            var xMin = Math.Max(0, ship.X - 1);
            var yMax = Math.Min(Size - 1, ship.Y + (ship.Orientation == Orientation.Vertical   ? ship.Length : 1));
            var xMax = Math.Min(Size - 1, ship.X + (ship.Orientation == Orientation.Horizontal ? ship.Length : 1));
            for (int y = yMin; y <= yMax; y++)
                for (int x = xMin; x <= xMax; x++)
                    _grid[x, y] = ShipStatus.Water;

            // Mark ship.
            yMin = ship.Y;
            xMin = ship.X;
            yMax = ship.Y + (ship.Orientation == Orientation.Vertical   ? ship.Length - 1 : 0);
            xMax = ship.X + (ship.Orientation == Orientation.Horizontal ? ship.Length - 1 : 0);
            for (int y = yMin; y <= yMax; y++)
                for (int x = xMin; x <= xMax; x++)
                    _grid[x, y] = ShipStatus.Ship;
        }

        /// <summary>
        /// Marks all unmarked water as clear.
        /// </summary>
        private void FillSpace()
        {
            foreach (var cell in _grid)
                if (_grid[cell] == ShipStatus.Unspecified)
                    _grid[cell] = ShipStatus.Water;
        }

        /// <summary>
        /// Launches an attack at a specified grid reference.
        /// </summary>
        /// <param name="cell">The coordinates of the attack.</param>
        /// <returns>The results of the attack.</returns>
        public BattlefieldStatus Attack(Cell cell)
        {
            return new BattlefieldStatus
            {
                AttackResult = GetAttackResult(cell),
                Attacks = ++Attacks,
                Cell = cell,
                Victory = GetVictory(),
            };
        }

        /// <summary>
        /// Determines whether an attack missed, hit or hit and sunk a ship.
        /// </summary>
        /// <param name="cell">The coordinates of the attack.</param>
        /// <returns>The damage caused by the attack.</returns>
        private AttackResult GetAttackResult(Cell cell)
        {
            if (_grid[cell] == ShipStatus.Water)
            {
                return AttackResult.Miss;
            }
            else
            {
                _grid[cell] = ShipStatus.Wreck;
                foreach (var delta in Delta.CardinalDirections)
                {
                    var ship = cell;
                    bool done = false;
                    while(!done)
                    {
                        ship += delta;
                        if (!ship.IsValid(Battlefield.Size) || _grid[ship] == ShipStatus.Water)
                            done = true;
                        else if (_grid[ship] == ShipStatus.Ship)
                            return AttackResult.Hit;
                    }
                }
                return AttackResult.Sunk;
            }
        }

        /// <summary>
        /// Determines whether the all ships are completely wrecked.
        /// </summary>
        /// <returns>True if all ships are completely wrecked; otherwise, false.</returns>
        private bool GetVictory()
        {
            return _grid.All(cell => _grid[cell] != ShipStatus.Ship);
        }
    }
}
