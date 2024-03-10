using System;
using System.Collections.Generic;
using System.Linq;

namespace PatherySolver
{
    /// <summary>
    /// A map.
    /// </summary>
    public class Map : ICloneable
    {
        /// <summary>
        /// This map's cells.
        /// </summary>
        private readonly char[,] cells;

        /// <summary>
        /// Gets an individual cell on this map.
        /// </summary>
        /// <param name="location">The cell's location.</param>
        /// <returns>The cell at the specified location.</returns>
        public char this[Location location] => cells[location.X, location.Y];

        /// <summary>
        /// Gets an individual cell on this map.
        /// </summary>
        /// <param name="x">The cell's x coordinate.</param>
        /// <param name="y">The cell's y coordinate.</param>
        /// <returns>The cell at the specified coordinates.</returns>
        public char this[int x, int y] => cells[x, y];

        /// <summary>
        /// The number of bots on this map.
        /// </summary>
        public int Bots { get; }

        /// <summary>
        /// The number of checkpoints to hit before walking to the exit.
        /// </summary>
        public int Checkpoints { get; }

        /// <summary>
        /// This map's height;
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// The number of moves required to solve this map for each bot.
        /// </summary>
        public int[] Moves { get; }

        /// <summary>
        /// The out location of each teleporter.
        /// </summary>
        public Location[] Outs { get; }

        /// <summary>
        /// The total number of moves required to solve this map.
        /// </summary>
        public int TotalMoves => Moves.Sum();

        /// <summary>
        /// The number of walls the user may add to this map.
        /// </summary>
        public int Walls { get; private set; }

        /// <summary>
        /// This map's width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Clones the specified map.
        /// </summary>
        /// <param name="clone">The map to clone.</param>
        private Map(Map clone)
        {
            cells = (char[,])clone.cells.Clone();
            Checkpoints = clone.Checkpoints;
            Height = clone.Height;
            Moves = (int[])clone.Moves.Clone();
            Outs = (Location[])clone.Outs.Clone();
            Bots = clone.Bots;
            Walls = clone.Walls;
            Width = clone.Width;
        }

        /// <summary>
        /// Validates and initialises a new map.
        /// </summary>
        /// <param name="walls">The number of walls the user may add.</param>
        /// <param name="lines">The cells of the map.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="walls"/> is negative, or
        /// <paramref name="lines"/> is invalid for one of the following reasons:
        /// <list type="bullet">
        /// <item>Not all lines have the same width.</item>
        /// <item>The perimeter is not made entirely of fixed walls.</item>
        /// <item>There is no exit.</item>
        /// <item>Starting points are not specified in consecutive order starting from the first one.</item>
        /// <item>Checkpoints are not specified in consecutive order starting from 'A'.</item>
        /// <item>A teleporter has an in or an out but not both.</item>
        /// </list>
        /// </exception>
        public Map(int walls, params string[] lines)
        {
            if (walls < 0)
                throw new ArgumentException("Cannot have a negative number of walls.", nameof(walls));
            Walls = walls;

            Width = lines[0].Length;
            if (lines.Any(line => line.Length != Width))
                throw new ArgumentException("All lines must have the same length.", nameof(lines));

            Height = lines.Length;
            cells = new char[Width, Height];

            var bots = new bool[Cell.Start.Length];
            var checkpoints = new bool[Cell.Checkpoint.Length];
            var hasExit = false;
            var ins = new bool[Cell.In.Length];
            Outs = new Location[Cell.Out.Length];

            int xMax = Width - 1;
            int yMax = Height - 1;
            for (int y = 0; y <= yMax; y++)
            {
                var line = lines[y];
                for (int x = 0; x <= xMax; x++)
                {
                    char cell = line[x];
                    if ((y == 0 || y == yMax || x == 0 || x == xMax) && cell != Cell.FixedWall)
                        throw new ArgumentException("The map's perimiter must be fixed walls ('" + Cell.FixedWall + "')", nameof(lines));
                    cells[x, y] = cell;

                    int i;
                    if (Cell.IsStart(cell, out i))
                        bots[i] = true;
                    else if (Cell.Exit == cell)
                        hasExit = true;
                    else if (Cell.IsCheckpoint(cell, out i))
                        checkpoints[i] = true;
                    else if (Cell.IsIn(cell, out i))
                        ins[i] = true;
                    else if (Cell.IsOut(cell, out i))
                        Outs[i] = new Location(x, y);
                }
            }

            if (!hasExit)
                throw new ArgumentException("There is no exit.", nameof(lines));

            Bots = bots.TakeWhile(bot => bot).Count();
            if (bots.Skip(Bots).Any(walker => walker))
                throw new ArgumentException($"Starting points are not in consecutive order starting from the first one.", nameof(lines));

            Checkpoints = checkpoints.TakeWhile(checkpoint => checkpoint).Count();
            if (checkpoints.Skip(Checkpoints).Any(checkpoint => checkpoint))
                throw new ArgumentException($"Checkpoints ('A'-'Z') are not in consecutive order starting from 'A'.", nameof(lines));

            for (int i = 0; i < ins.Length; i++)
                if (ins[i] == (Outs[i] == Location.Empty))
                    throw new ArgumentException("A teleporter does not have both its in and out.");

            Moves = new int[Bots];
        }

        /// <summary>
        /// Adds a wall to the map.
        /// </summary>
        /// <param name="location">The wall's location.</param>
        /// <returns>True if the wall was successfully added.
        /// False if a wall cannot be added at that location.</returns>
        /// <exception cref="InvalidOperationException"><see cref="Walls"/> is 0.</exception>
        public bool AddWall(Location location) => AddWall(location.X, location.Y);

        /// <summary>
        /// Adds a wall to the map.
        /// </summary>
        /// <param name="x">The wall's x coordinate.</param>
        /// <param name="y">The wall's y coordinate.</param>
        /// <returns>True if the wall was successfully added.
        /// False if a wall cannot be added at that location.</returns>
        /// <exception cref="InvalidOperationException"><see cref="Walls"/> is 0.</exception>
        public bool AddWall(int x, int y)
        {
            if (Walls <= 0)
                throw new InvalidOperationException("More walls cannot be added.");

            if (cells[x, y] != Cell.Empty)
                return false;

            cells[x, y] = Cell.UserWall;
            Walls--;
            return true;
        }

        /// <summary>
        /// Creates a new <see cref="Map"/> that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="Map"/> that is a copy of this instance.</returns>
        public Map Clone() => new(this);
        /// <inheritdoc/>
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Finds all locations with the specified cell.
        /// </summary>
        /// <param name="cell">The cell to find.</param>
        /// <returns>All locations with the specified cell.</returns>
        public IEnumerable<Location> Find(char cell)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    if (cells[x, y] == cell)
                        yield return new Location(x, y);
        }
    }
}
