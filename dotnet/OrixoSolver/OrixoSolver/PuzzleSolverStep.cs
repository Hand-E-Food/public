using System.Collections.Generic;

namespace OrixoSolver
{
    public class PuzzleSolverStep
    {
        public Direction Direction { get; }

        public NumberCell NumberCell { get; }

        public ICollection<SpaceCell> SpaceCells { get; }

        public PuzzleSolverStep(NumberCell numberCell, Direction direction, ICollection<SpaceCell> spaceCells)
        {
            Direction = direction;
            NumberCell = numberCell;
            SpaceCells = spaceCells;
        }
    }
}
