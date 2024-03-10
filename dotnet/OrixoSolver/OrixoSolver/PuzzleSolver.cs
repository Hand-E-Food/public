using System.Collections.Generic;
using System.Linq;

namespace OrixoSolver
{
    public class PuzzleSolver
    {
        private readonly HashSet<Cell> dirtyCells;

        public Puzzle Puzzle { get; }

        public PuzzleSolver(Puzzle puzzle)
        {
            puzzle.Initialise();
            Puzzle = puzzle;
            dirtyCells = new HashSet<Cell>(puzzle.Cells);
        }

        public IEnumerable<PuzzleSolverStep> Solve()
        {
            PuzzleSolverStep step;
            while (dirtyCells.Count > 0)
            {
                var dirtyCell = dirtyCells.First();
                dirtyCells.Remove(dirtyCell);

                if (dirtyCell.IsUsed)
                    step = null;
                else if (dirtyCell is SpaceCell spaceCell)
                    step = Solve(spaceCell);
                else if (dirtyCell is NumberCell numberCell)
                    step = Solve(numberCell);
                else
                    step = null;

                if (step != null)
                    yield return step;
            }
        }

        private PuzzleSolverStep Solve(NumberCell numberCell)
        {
            var origin = From(numberCell);

            var removedDirections = new HashSet<Direction>();
            foreach (var (direction, viableCells) in numberCell.ViableDirections.ToList())
            {
                if (viableCells.Count < numberCell.Number)
                {
                    removedDirections.Add(direction);
                }
                else if (viableCells.Count == numberCell.Number || viableCells.Any(spaceCell => spaceCell.ViableCells.HasOnly(numberCell)))
                {
                    foreach (var otherDirection in numberCell.ViableDirections.Keys.Except(new[] { direction }).ToList())
                        removedDirections.Add(otherDirection);
                }
            }

            if (removedDirections.Count > 0)
                origin.Remove(removedDirections);

            if (numberCell.ViableDirections.TrySingle(out var pair))
            {
                var direction = pair.Key;
                var viableCells = pair.Value;

                if (viableCells.Count == numberCell.Number)
                    return origin.UseIn(direction);
            }

            return null;
        }

        private PuzzleSolverStep Solve(SpaceCell spaceCell)
        {
            if (spaceCell.ViableCells.TrySingle(out var numberCell) && numberCell.ViableDirections.Count > 1)
            {
                var direction = numberCell.Location.GetDirectionTo(spaceCell.Location);
                From(numberCell).KeepOnly(direction);
            }

            return null;
        }

        private NumberCellOrigin From(NumberCell numberCell) => new NumberCellOrigin(this, numberCell);

        private SpaceCellOrigin From(SpaceCell spaceCell) => new SpaceCellOrigin(this, spaceCell);

        private class NumberCellOrigin
        {
            private readonly PuzzleSolver solver;
            private readonly NumberCell numberCell;

            public NumberCellOrigin(PuzzleSolver solver, NumberCell numberCell)
            {
                this.solver = solver;
                this.numberCell = numberCell;
            }

            public void KeepOnly(Direction direction)
            {
                Remove(numberCell.ViableDirections.Keys.Except(new[] { direction }));
            }

            public void Remove(IEnumerable<Direction> directions)
            {
                foreach (var direction in directions)
                    Remove(direction);
            }

            public void Remove(Direction direction)
            {
                if (numberCell.ViableDirections.TryGetValue(direction, out var spaceCells))
                {
                    numberCell.ViableDirections.Remove(direction);
                    if (spaceCells.Count > 0)
                    {
                        solver.dirtyCells.Add(numberCell);
                        foreach (var spaceCell in spaceCells)
                            solver.From(spaceCell).Remove(numberCell);
                    }
                }
            }

            public void Remove(SpaceCell spaceCell)
            {
                var direction = numberCell.Location.GetDirectionTo(spaceCell.Location);
                if (numberCell.ViableDirections.TryGetValue(direction, out var spaceCells) && spaceCells.Remove(spaceCell))
                {
                    solver.dirtyCells.Add(numberCell);
                    solver.From(spaceCell).Remove(numberCell);
                    if (spaceCells.Count == 0)
                        numberCell.ViableDirections.Remove(direction);
                }
            }

            public PuzzleSolverStep UseIn(Direction direction)
            {
                numberCell.IsUsed = true;
                var location = numberCell.Location;
                var spaceCells = new List<SpaceCell>(numberCell.Number);
                for (int n = numberCell.Number; n > 0; n--)
                {
                    SpaceCell spaceCell = null;
                    while (spaceCell == null)
                    {
                        location += direction;
                        var cell = solver.Puzzle[location];
                        if (cell is SpaceCell && !cell.IsUsed)
                            spaceCell = (SpaceCell)cell;
                    }
                    solver.From(spaceCell).AssignTo(numberCell);
                    spaceCells.Add(spaceCell);
                }

                return new PuzzleSolverStep(numberCell, direction, spaceCells);
            }
        }

        private class SpaceCellOrigin
        {
            private readonly PuzzleSolver solver;
            private readonly SpaceCell spaceCell;

            public SpaceCellOrigin(PuzzleSolver solver, SpaceCell spaceCell)
            {
                this.solver = solver;
                this.spaceCell = spaceCell;
            }

            public void AssignTo(NumberCell numberCell)
            {
                spaceCell.IsUsed = true;
                var otherNumberCells = spaceCell.ViableCells.Except(new[] { numberCell }).ToList();
                foreach (var otherNumberCell in otherNumberCells)
                    Remove(otherNumberCell);
            }

            public void Remove(NumberCell numberCell)
            {
                if (spaceCell.ViableCells.Remove(numberCell))
                {
                    solver.dirtyCells.Add(spaceCell);
                    solver.From(numberCell).Remove(spaceCell);
                }
            }
        }
    }
}
