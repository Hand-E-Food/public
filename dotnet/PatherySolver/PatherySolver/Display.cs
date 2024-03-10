using System;
using System.Collections.Generic;

namespace PatherySolver
{
    public class Display
    {
        public void Write(Map map, int top)
        {
            if (map == null) return;

            lock (Console.Out)
            {
                Console.SetCursorPosition(0, top);
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        var format = Format[map[x, y]];
                        Console.ForegroundColor = format.ForegroundColor;
                        Console.BackgroundColor = format.BackgroundColor;
                        Console.Write(format.Character);
                    }
                    Console.WriteLine();
                }
                Console.ResetColor();
                Console.Write($"{map.Walls} walls   ");
                if (map.Bots > 1)
                {
                    for (int i = 0; i < map.Bots; i++)
                    {
                        if (i > 0)
                            Console.Write("+");

                        Console.ForegroundColor = Format[Cell.Start[i]].ForegroundColor;
                        Console.Write(map.Moves[i]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.Write("= ");
                }
                Console.WriteLine($"{map.TotalMoves} moves    ");
            }
        }

        private static Dictionary<char, CellFormat> Format = InitialiseFormat();
        private static Dictionary<char, CellFormat> InitialiseFormat()
        {
            var formats = new Dictionary<char, CellFormat>
            {
                { Cell.Empty     , new CellFormat(' ', ConsoleColor.White      , ConsoleColor.White      ) },
                { Cell.Exit      , new CellFormat('x', ConsoleColor.DarkGray   , ConsoleColor.Gray       ) },
                { Cell.Gate[0]   , new CellFormat('+', ConsoleColor.Green      , ConsoleColor.Gray       ) },
                { Cell.Gate[1]   , new CellFormat('+', ConsoleColor.Red        , ConsoleColor.Gray       ) },
                { Cell.FixedEmpty, new CellFormat('░', ConsoleColor.Gray       , ConsoleColor.White      ) },
                { Cell.FixedWall , new CellFormat('≡', ConsoleColor.Yellow     , ConsoleColor.DarkYellow ) },
                { Cell.Ice       , new CellFormat('░', ConsoleColor.Blue       , ConsoleColor.Cyan       ) },
                { Cell.Start[0]  , new CellFormat('►', ConsoleColor.Green      , ConsoleColor.Gray       ) },
                { Cell.Start[1]  , new CellFormat('◄', ConsoleColor.Red        , ConsoleColor.Gray       ) },
                { Cell.UserWall  , new CellFormat('+', ConsoleColor.White      , ConsoleColor.DarkYellow ) },
            };

            for(int i = 0; i < Cell.Checkpoint.Length; i++)
                formats.Add(Cell.Checkpoint[i], new CellFormat(Cell.Checkpoint[i], ConsoleColor.White, (i % 4) switch {
                    0 => ConsoleColor.DarkBlue,
                    1 => ConsoleColor.DarkMagenta,
                    2 => ConsoleColor.DarkCyan,
                    3 => ConsoleColor.DarkRed,
                    _ => throw new Exception("Unreachable code reached."),
                }));

            for (int i = 0; i < Cell.In.Length; i++)
            {
                var backgroundColor = (i % 12) switch {
                    0 => ConsoleColor.DarkBlue,
                    1 => ConsoleColor.DarkGreen,
                    2 => ConsoleColor.DarkCyan,
                    3 => ConsoleColor.DarkRed,
                    4 => ConsoleColor.DarkMagenta,
                    5 => ConsoleColor.DarkYellow,
                    6 => ConsoleColor.Blue,
                    7 => ConsoleColor.Green,
                    8 => ConsoleColor.Cyan,
                    9 => ConsoleColor.Red,
                    10 => ConsoleColor.Magenta,
                    11 => ConsoleColor.Yellow,
                    _ => throw new Exception("Unreachable code reached."),
                };

                var foregroundColor = backgroundColor.HasFlag(ConsoleColor.DarkGray)
                    ? ConsoleColor.Black
                    : ConsoleColor.White;

                formats.Add(Cell.In[i] , new CellFormat('i', foregroundColor, backgroundColor));
                formats.Add(Cell.Out[i], new CellFormat('o', foregroundColor, backgroundColor));
            }
            return formats;
        }

        private class CellFormat
        {
            public readonly ConsoleColor BackgroundColor;
            public readonly char Character;
            public readonly ConsoleColor ForegroundColor;

            public CellFormat(char character, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            {
                Character = character;
                ForegroundColor = foregroundColor;
                BackgroundColor = backgroundColor;
            }
        }
    }
}
