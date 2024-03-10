using System.Collections.Generic;

namespace OrixoSolver
{
    public class SpaceCell : Cell
    {
        public List<NumberCell> ViableCells = new List<NumberCell>();

        public SpaceCell(Point location) : base(location)
        { }
    }
}
