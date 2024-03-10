using MarkRichardson.Battleship.Navy;
using System;
using System.Linq;

namespace MarkRichardson.Battleship.Gunners.ScannerMk2
{
    public class Gunner : IGunner
    {
        private Battlefield _battlefield;
        private Grid<AttackResult> _grid = new Grid<AttackResult>(Battlefield.Size);
        private Cell _scan = new Cell();

        public Gunner(Battlefield battlefield)
        {
            _battlefield = battlefield;
        }

        public BattlefieldStatus Fire()
        {
            Cell target;
            if (_grid[_scan] == AttackResult.Hit)
                target = AttackShip();
            else
                target = AttackUnknown();

            var result = _battlefield.Attack(target);
            RecordAttack(result);
            return result;
        }

        /// <summary>
        /// Continues the attack on a hit ship.
        /// </summary>
        /// <returns>The cell to attack.</returns>
        private Cell AttackShip()
        {
            foreach (var delta in Delta.CardinalDirections)
            {
                var ship = _scan;
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
        private Cell AttackUnknown()
        {
            while (_grid[_scan] != AttackResult.Unknown)
            {
                _scan.X = (_scan.X + 7);
                if (_scan.X >= Battlefield.Size)
                {
                    _scan.X = _scan.X % Battlefield.Size;
                    _scan.Y = (_scan.Y + 1) % Battlefield.Size;
                }
            }
            return _scan;
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
                bool done = false;
                while (!done)
                {
                    ship += delta;
                    if (!ship.IsValid(Battlefield.Size))
                        done = true;
                    else if (_grid[ship] == AttackResult.Hit)
                        _grid[ship] = AttackResult.Sunk;
                    else
                    {
                        _grid[ship] = AttackResult.Miss;
                        done = true;
                    }
                }
            }
        }
    }
}
