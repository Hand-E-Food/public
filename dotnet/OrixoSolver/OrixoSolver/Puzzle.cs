using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace OrixoSolver
{
    public class Puzzle
    {
        private SortedDictionary<Point, Cell> cells = new SortedDictionary<Point, Cell>(PointComparer.Instance);

        public Cell this[Point location] => cells.TryGetValue(location, out var cell) ? cell : null;

        public IEnumerable<Cell> Cells => cells.Values;

        public bool IsSolved => Cells.All(cell => cell.IsUsed);

        public string Name { get; }

        public IEnumerable<NumberCell> NumberCells => Cells.OfType<NumberCell>();

        public IEnumerable<SpaceCell> SpaceCells => Cells.OfType<SpaceCell>();

        public Puzzle(string name, IList<string> map)
        {
            Name = name;

            char cellName = 'A';
            for(int y = 0; y < map.Count; y++)
            {
                var line = map[y];
                for (int x = 0; x < line.Length; x++)
                {
                    char character = line[x];
                    Cell cell;
                    if (character == '.')
                        cell = new SpaceCell(new Point(x, y));
                    else if (character >= '1' && character <= '9')
                        cell = new NumberCell(new Point(x, y), character - '0', cellName++);
                    else if (character >= 'a' && character <= 'z')
                        cell = new NumberCell(new Point(x, y), character - 'a' + 10, cellName++);
                    else
                        continue;

                    cells.Add(cell.Location, cell);
                }
            }
        }

        public void Initialise()
        {
            foreach (var numberCell in NumberCells)
                Initialise(numberCell);
        }

        private void Initialise(NumberCell numberCell)
        {
            foreach (var pair in numberCell.ViableDirections)
            {
                var direction = pair.Key;
                var viableCells = pair.Value;
                var location = numberCell.Location;
                while (true)
                {
                    location += direction;
                    if (!cells.TryGetValue(location, out var cell)) break;
                    if (cell is SpaceCell spaceCell)
                    {
                        viableCells.Add(spaceCell);
                        spaceCell.ViableCells.Add(numberCell);
                    }
                }
            }
        }

        private class PointComparer : IComparer<Point>
        {
            public static readonly PointComparer Instance = new PointComparer();

            private PointComparer()
            { }

            public int Compare([AllowNull] Point left, [AllowNull] Point right)
            {
                int result = 0;
                if (result == 0) result = left.Y - right.Y;
                if (result == 0) result = left.X - right.X;
                return result;
            }
        }
    }
}
