using MarkRichardson.Battleship.Navy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkRichardson.Battleship.Gunners.HeatMapperMk4
{
    public class Gunner : IGunner
    {
        private Battlefield _battlefield;
        private Grid<AttackResult> _grid = new Grid<AttackResult>(Battlefield.Size);
        private List<int> _shipLengths = Battlefield.ShipLengths.ToList();

        public Gunner(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        public BattlefieldStatus Fire()
        {
            Cell? target;
            target = AttackShip();
            if (!target.HasValue)
                target = AttackUnknown();
            var result = _battlefield.Attack(target.Value);
            RecordAttack(result);
            return result;
        }

        /// <summary>
        /// Continues the attack on a hit ship.
        /// </summary>
        /// <returns>The cell to attack.</returns>
        private Cell? AttackShip()
        {
            if (_grid.All(cell => _grid[cell] != AttackResult.Hit))
                return null;

            var origin = _grid.FirstOrDefault(cell => _grid[cell] == AttackResult.Hit);

            Cell? longestCell = null;
            var longestLength = 0;
            foreach (var delta in Delta.CardinalDirections)
            {
                var ship = origin;
                Cell? firstCell = null;
                var length = 0;
                bool done = false;
                while (!done)
                {
                    ship += delta;
                    if (!ship.IsValid(Battlefield.Size) || _grid[ship] == AttackResult.Miss)
                        done = true;
                    else if (_grid[ship] == AttackResult.Unknown)
                    {
                        if (!firstCell.HasValue)
                            firstCell = ship;
                        length++;
                    }
                }
                if (firstCell.HasValue && longestLength < length)
                {
                    longestLength = length;
                    longestCell = firstCell.Value;
                }
            }

            return longestCell;
        }

        /// <summary>
        /// Scans for an unknown cell to attack.
        /// </summary>
        /// <returns>The cell to attack.</returns>
        private Cell? AttackUnknown()
        {
            var currentHeatMap = CreateHeatMap(_grid, _shipLengths);
            return currentHeatMap
                .OrderByDescending(cell => currentHeatMap[cell])
                .First();
        }

        /// <summary>
        /// Records the attack's results.
        /// </summary>
        /// <param name="result">The attack's results.</param>
        private void RecordAttack(BattlefieldStatus result)
        {
            _grid[result.Cell] = result.AttackResult;

            if ((result.AttackResult & AttackResult.Hit) != 0)
                RecordHit(result.Cell);

            if (result.AttackResult == AttackResult.Sunk)
                RecordSunk(result.Cell);
        }

        /// <summary>
        /// Marks the cells diagonally adjacent to the hit cell as water.
        /// </summary>
        /// <param name="cell">The cell that was hit.</param>
        private void RecordHit(Cell cell)
        {
            var cells = Delta.DiagonalDirections
                .Select(d => cell + d)
                .Where(x => x.IsValid(Battlefield.Size));

            foreach (var waterCell in cells)
                _grid[waterCell] = AttackResult.Miss;
        }

        /// <summary>
        /// Records that an entire ship was sunk.
        /// </summary>
        /// <param name="cell">The cell that was hit.</param>
        private void RecordSunk(Cell cell)
        {
            foreach (var delta in Delta.CardinalDirections)
            {
                var ship = cell;
                var length = 1;
                bool done = false;
                while (!done)
                {
                    ship += delta;
                    if (!ship.IsValid(Battlefield.Size))
                    {
                        done = true;
                    }
                    else if (_grid[ship] == AttackResult.Hit)
                    {
                        _grid[ship] = AttackResult.Sunk;
                        length++;
                    }
                    else
                    {
                        _grid[ship] = AttackResult.Miss;
                        done = true;
                    }
                }
                _shipLengths.Remove(length);
            }
        }

        /// <summary>
        /// Generates a heat map of the specified grid.
        /// </summary>
        /// <param name="grid">The grid to analyse.</param>
        /// <param name="shipLengths">The lengths of the remaining ships.</param>
        /// <returns>A heat map for the specified grid.</returns>
        private static Grid<int> CreateHeatMap(Grid<AttackResult> grid, IEnumerable<int> shipLengths)
        {
            var heat = new Grid<int>(grid.Size);
            var ships = new List<Ship>();
            foreach (var length in shipLengths)
            {
                for (int y = 0; y < grid.Size; y++)
                    for (int x = 0; x <= grid.Size - length; x++)
                        ships.Add(new Ship(x, y, length, Orientation.Horizontal));
                for (int y = 0; y <= grid.Size - length; y++)
                    for (int x = 0; x < grid.Size; x++)
                        ships.Add(new Ship(x, y, length, Orientation.Vertical));
            }

            int count = ships.RemoveAll(ship =>
                ship.Any(cell =>
                    grid[cell] != AttackResult.Unknown));

            foreach (var ship in ships)
                foreach (var cell in ship)
                    heat[cell]++;

            return heat;
        }
    }
}
