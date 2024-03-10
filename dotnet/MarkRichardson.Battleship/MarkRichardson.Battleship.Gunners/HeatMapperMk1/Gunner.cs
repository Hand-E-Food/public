using MarkRichardson.Battleship.Navy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkRichardson.Battleship.Gunners.HeatMapperMk1
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

            foreach (var delta in Delta.CardinalDirections)
            {
                var ship = origin;
                bool done = false;
                while (!done)
                {
                    ship += delta;
                    if (!ship.IsValid(Battlefield.Size) || _grid[ship] == AttackResult.Miss)
                        done = true;
                    else if (_grid[ship] == AttackResult.Unknown)
                        return ship;
                }
            }
            throw new ApplicationException("Map records are in an invalid state.");
        }

        /// <summary>
        /// Scans for an unknown cell to attack.
        /// </summary>
        /// <returns>The cell to attack.</returns>
        private Cell? AttackUnknown()
        {
            var heat = new Grid<int>(_grid.Size);
            var ships = new List<Ship>();
            foreach (var length in _shipLengths)
            {
                for (int y = 0; y < _grid.Size; y++)
                    for (int x = 0; x <= _grid.Size - length; x++)
                        ships.Add(new Ship(x, y, length, Orientation.Horizontal));
                for (int y = 0; y <= _grid.Size - length; y++)
                    for (int x = 0; x < _grid.Size; x++)
                        ships.Add(new Ship(x, y, length, Orientation.Vertical));
            }

            int count = ships.RemoveAll(ship =>
                ship.Any(cell =>
                    _grid[cell] != AttackResult.Unknown));

            foreach (var ship in ships)
                foreach (var cell in ship)
                    heat[cell]++;

            return heat
                .OrderByDescending(cell => heat[cell])
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
    }
}
