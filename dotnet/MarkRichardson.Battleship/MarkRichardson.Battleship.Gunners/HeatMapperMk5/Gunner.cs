using MarkRichardson.Battleship.Navy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkRichardson.Battleship.Gunners.HeatMapperMk5
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
            var heatMap = CreateHeatMap(_grid, _shipLengths);
            Cell? target;
            target = AttackShip(heatMap);
            if (!target.HasValue)
                target = AttackUnknown(heatMap);
            var result = _battlefield.Attack(target.Value);
            RecordAttack(result);
            return result;
        }

        /// <summary>
        /// Continues the attack on a hit ship.
        /// </summary>
        /// <param name="heatMap">The heat map of the current battlefield.</param>
        /// <returns>The cell to attack.</returns>
        private Cell? AttackShip(HeatMap heatMap)
        {
            var hitCells = _grid.Where(cell => _grid[cell] == AttackResult.Hit).ToArray();
            if (!hitCells.Any())
                return null;

            return hitCells
                .SelectMany(cell => Delta.CardinalDirections.Select(delta => cell + delta))
                .Where(cell => cell.IsValid(_grid.Size) && _grid[cell] == AttackResult.Unknown)
                .OrderByDescending(cell => heatMap[cell])
                .First();
        }

        /// <summary>
        /// Scans for an unknown cell to attack.
        /// </summary>
        /// <param name="heatMap">The heat map of the current battlefield.</param>
        /// <returns>The cell to attack.</returns>
        private Cell? AttackUnknown(HeatMap heatMap)
        {
            return heatMap
                .Where(cell => _grid[cell] == AttackResult.Unknown)
                .OrderByDescending(cell => heatMap[cell])
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
        private static HeatMap CreateHeatMap(Grid<AttackResult> grid, IEnumerable<int> shipLengths)
        {
            var heat = new HeatMap(grid.Size);
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
                    grid[cell] == AttackResult.Miss));

            foreach (var ship in ships)
                foreach (var cell in ship)
                    heat[cell]++;

            return heat;
        }

        /// <summary>
        /// The signature of a heat map.
        /// </summary>
        private class HeatMap : Grid<int>
        {
            public HeatMap(int size)
                : base(size)
            { }
        }
    }
}
