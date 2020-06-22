using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Actors.Mobs;
using PsiFi.Models.Mapping.Geometry;
using System;

namespace PsiFi.Views.ColorConsole
{
    /// <summary>
    /// Displays a map on the console.
    /// </summary>
    class MapView : IMapView
    {
        /// <inheritdoc/>
        public Map Map
        {
            get => map;
            set
            {
                map = value;
                Clear();
                if (map != null)
                    Refresh();
            }
        }
        private Map map = null;
       
        /// <summary>
        /// Clears the console.
        /// </summary>
        private static void Clear()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        /// <summary>
        /// Draws a cell according to <paramref name="appearance"/> at the current cursor location.
        /// </summary>
        /// <param name="appearance">The cell's appearance.</param>
        private static void Draw(Appearance appearance)
        {
            Console.ForegroundColor = appearance.ForeColor;
            Console.BackgroundColor = appearance.BackColor;
            Console.Write(appearance.Character);
        }

        /// <summary>
        /// Gets input from the player.
        /// </summary>
        /// <returns>The player's selected action.</returns>
        public MapViewResponse GetPlayerInput()
        {
            return new KeyActionCollection<MapViewResponse>
            {
                { new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false), () => MapViewResponse.Move(Direction.N) },
                { new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false), () => MapViewResponse.Move(Direction.W) },
                { new ConsoleKeyInfo('s', ConsoleKey.S, false, false, false), () => MapViewResponse.Move(Direction.S) },
                { new ConsoleKeyInfo('d', ConsoleKey.D, false, false, false), () => MapViewResponse.Move(Direction.E) },
                { new ConsoleKeyInfo('.', ConsoleKey.OemPeriod, false, false, false), () => MapViewResponse.Wait },
            }.ReadKey();
        }

        /// <summary>
        /// Draws the entire map.
        /// </summary>
        private void Refresh()
        {
            for (int y = 0; y < map.Size.Height; y++)
            {
                for(int x = 0; x < map.Size.Width; x++)
                {
                    var cell = map.Cells[x, y];
                    cell.HasChanged = false;
                    Draw(cell.Appearance);
                }
                if (Console.CursorLeft > 0) Console.WriteLine();
            }
        }

        /// <summary>
        /// Draws all cells that have changed since last being drawn.
        /// </summary>
        public void Update()
        {
            for (int y = 0; y < map.Size.Height; y++)
            {
                for (int x = 0; x < map.Size.Width; x++)
                {
                    var cell = map.Cells[x, y];
                    if (cell.HasChanged)
                    {
                        cell.HasChanged = false;
                        Console.SetCursorPosition(cell.Location.X, cell.Location.Y);
                        Draw(cell.Appearance);
                    }
                }
            }
        }
    }
}
