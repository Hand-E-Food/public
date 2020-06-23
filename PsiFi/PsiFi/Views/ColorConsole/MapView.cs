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

        public void ClearMessage()
        {
            Console.SetCursorPosition(0, map.Size.Height + 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(new string(' ', map.Size.Width));
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

        public void Message(string message)
        {
            ClearMessage();
            Console.SetCursorPosition(0, map.Size.Height + 1);
            Console.Write(message);
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

        /// <summary>
        /// Gets input from the player.
        /// </summary>
        /// <returns>The player's selected action.</returns>
        public MapViewResponse GetPlayerInput()
        {
            MapViewResponse response;
            do
            {
                response = new KeyActionCollection<MapViewResponse>
                {
                    { ConsoleKey.W, () => MapViewResponse.Move(Direction.N) },
                    { ConsoleKey.A, () => MapViewResponse.Move(Direction.W) },
                    { ConsoleKey.S, () => MapViewResponse.Move(Direction.S) },
                    { ConsoleKey.D, () => MapViewResponse.Move(Direction.E) },
                    { ConsoleKey.OemPeriod, () => MapViewResponse.Wait },
                    { new KeyInfo(ConsoleModifiers.Shift, ConsoleKey.Q), ConfirmQuit }
                }.ReadKey();
            }
            while (response == null);
            return response;
        }

        private MapViewResponse ConfirmQuit()
        {
            Message("Really quit (y/n)?");
            var response = new KeyActionCollection<MapViewResponse>
            {
                { ConsoleKey.Y, () => MapViewResponse.Quit },
                { ConsoleKey.N, () => null },
            }.ReadKey();
            ClearMessage();
            return response;
        }
    }
}
