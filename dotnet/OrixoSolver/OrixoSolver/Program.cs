using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OrixoSolver
{
    class Program
    {
        private static readonly Dictionary<Direction, char> Arrow = new Dictionary<Direction, char>
        {
            { Direction.E, '>' },
            { Direction.N, '^' },
            { Direction.S, 'v' },
            { Direction.W, '<' },
        };

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            using var puzzleStream = File.OpenRead("puzzles.txt");
            using var puzzleReader = new PuzzleReader(puzzleStream);
            while (true)
            {
                var puzzle = puzzleReader.ReadPuzzle();
                if (puzzle == null) break;
                Solve(puzzle);
            }
        }

        private static void Solve(Puzzle puzzle)
        {
            var solver = new PuzzleSolver(puzzle);
            Write(puzzle);
            WaitForKeyPress();
            foreach (var step in solver.Solve())
            {
                Write(step, ConsoleColor.Red);
                WaitForKeyPress();
                Write(step, ConsoleColor.Blue);
            }
            WaitForKeyPress();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void WaitForKeyPress() => Console.ReadKey(intercept: true);

        private static void Write(Puzzle puzzle)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            var cells = puzzle.Cells
                .OrderBy(cell => cell.Location.Y)
                .ThenBy(cell => cell.Location.X);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(puzzle.Name);

            foreach (var cell in cells)
                Write(cell, ConsoleColor.Blue);
        }

        private static void Write(PuzzleSolverStep step, ConsoleColor color)
        {
            Write(step.NumberCell, Arrow[step.Direction], color);
            foreach (var spaceCell in step.SpaceCells)
                Write(spaceCell, color);
        }

        private static void Write(Cell cell, ConsoleColor color)
        {
            char character;
            if (cell is NumberCell numberCell)
            {
                character = numberCell.Number < 10
                    ? (char)('0' + numberCell.Number)
                    : (char)('a' + numberCell.Number - 10);
            }
            else
            {
                character = ' ';
            }
            Write(cell, character, color);
        }

        private static void Write(Cell cell, char character, ConsoleColor color)
        {
            SetCursorPosition(cell.Location);
            Console.BackgroundColor = cell.IsUsed ? color : color & ConsoleColor.Gray;
            Console.Write(character);
        }

        private static void SetCursorPosition(Point location) => Console.SetCursorPosition(location.X, location.Y + 2);
    }
}
