using System.Collections.Generic;
using System.Linq;

namespace OrixoSolver
{
    public class NumberCell : Cell
    {
        public char Name { get; }

        public int Number { get; }

        public Dictionary<Direction, List<SpaceCell>> ViableDirections { get; }

        public NumberCell(Point location, int number, char name) : base(location)
        {
            Name = name;
            Number = number;
            ViableDirections = Direction.Cardinal.ToDictionary(
                direction => direction,
                direction => new List<SpaceCell>()
            );
        }
    }
}
