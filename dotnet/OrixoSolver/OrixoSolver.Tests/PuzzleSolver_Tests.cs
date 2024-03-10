using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace OrixoSolver.Tests
{
    public class PuzzleSolver_Tests
    {
        public class PuzzleInput
        {
            public string Name;
            public string[] Map;

            public PuzzleInput(string name, params string[] map)
            {
                Name = name;
                Map = map;
            }

            public override string ToString() => Name;

            public static implicit operator object[](PuzzleInput puzzleInput) => new object[] { puzzleInput.Name, puzzleInput.Map };
        }

        private static readonly Dictionary<Direction, char> Arrow = new Dictionary<Direction, char>
        {
            { Direction.E, '>' },
            { Direction.N, '^' },
            { Direction.S, 'v' },
            { Direction.W, '<' },
        };

        private readonly ITestOutputHelper testOutputHelper;

        public PuzzleSolver_Tests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        private void WriteLine(string message = "") => testOutputHelper.WriteLine(message);

        [Theory(DisplayName = "Solve puzzle"), MemberData(nameof(Puzzles))]
        public void SolvePuzzle(string name, string[] map)
        {
            foreach (var line in map)
                WriteLine(line);

            var puzzle = new Puzzle(name, map);
            var solver = new PuzzleSolver(puzzle);
            var steps = solver.Solve();
            
            WriteLine();
            foreach (var step in steps)
                WriteLine($"{step.NumberCell.Location.X:#0},{step.NumberCell.Location.Y:#0} {Arrow[step.Direction]} {step.NumberCell.Number}");
            
            WriteLine();
            for(int y = 0; y < map.Length; y++)
            {
                var line = map[y].ToCharArray();
                for(int x = 0; x < line.Length; x++)
                {
                    var cell = puzzle[new Point(x, y)];
                    if (cell != null && cell.IsUsed)
                        line[x] = '#';
                }
                WriteLine(new string(line));
            }

            Assert.True(puzzle.IsSolved);
        }

        public static IEnumerable<object[]> Puzzles()
        {
            return new object[][]
            {
                new PuzzleInput("1-01",
                    "2.."
                ),
                new PuzzleInput("1-02",
                    "2 2 2",
                    ". . .",
                    ". . ."
                ),
                new PuzzleInput("1-03",
                    " 1 ",
                    "2..",
                    " . "
                ),
                new PuzzleInput("1-04",
                    "  1 1 1",
                    "4......."
                ),
                new PuzzleInput("1-05",
                    "   . ",
                    ".3..2",
                    " . . ",
                    "3....",
                    " . 3 "
                ),
                new PuzzleInput("1-06",
                    " 3 1 ",
                    " ...2",
                    " .   ",
                    " .   ",
                    "..1  "
                ),
                new PuzzleInput("1-07",
                    " 1 ",
                    "1. ",
                    " .1",
                    "1. ",
                    " .1",
                    "1. ",
                    " .1",
                    " . "
                ),
                new PuzzleInput("1-08",
                    "  2",
                    "  .1",
                    "  .",
                    "22....."
                ),
                new PuzzleInput("1-09",
                    "1... ",
                    " .1. ",
                    " ..21",
                    " 2   "
                ),
                new PuzzleInput("1-10",
                    "1.1 1.1",
                    ". . . .",
                    "1.1 1.1",
                    "       ",
                    "6......",
                    " ....4 ",
                    "  2..  "
                ),
                new PuzzleInput("1-11",
                    "2...",
                    " .11",
                    " 2..",
                    "22..",
                    "  .."
                ),
            };
        }
    }
}
