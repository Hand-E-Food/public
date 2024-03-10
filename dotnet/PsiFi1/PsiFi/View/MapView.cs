using PsiFi.Models;
using System;

namespace PsiFi.View
{
    class MapView
    {
        private State? state;

        public void Initialise(State state)
        {
            if (state != null) Uninitialise();
            this.state = state ?? throw new ArgumentNullException(nameof(state));

            state.Player.HealthChanged += Player_HealthChanged;
            foreach (var cell in state.Map.AllCells)
                cell.AppearanceChanged += Cell_AppearanceChanged;

            var width  = state.Map.Width;
            var height = state.Map.Height + 10;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            Console.CursorVisible = false;

            RefreshAll();
        }

        public void RefreshAll()
        {
            Console.ResetColor();
            Console.Clear();
            RefreshMap();
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('═', Console.WindowWidth));
            RefreshPlayer();
        }

        private void Cell_AppearanceChanged(Cell cell)
        {
            Console.SetCursorPosition(cell.X, cell.Y);
            DrawCell(cell);
        }

        public void RefreshMap()
        {
            var map = state!.Map;

            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    DrawCell(map[x, y]);
                }
                if (Console.CursorLeft > 0)
                {
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }

        private void DrawCell(Cell cell)
        {
            var appearance = cell.Appearance;
            Console.ForegroundColor = appearance.ForegroundColor;
            Console.BackgroundColor = appearance.BackgroundColor;
            Console.Write(appearance.Character);
        }

        public void RefreshPlayer()
        {
            SetCursorForPlayerState(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Health: ");
            RefreshHealth();
        }

        private void Player_HealthChanged()
        {
            SetCursorForPlayerState(8, 0);
            RefreshHealth();
        }

        private void RefreshHealth()
        {
            var player = state!.Player;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(player.Health);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('/');
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(player.MaximumHealth);
            Console.ResetColor();
            Console.Write(new string(' ', 20 - Console.CursorLeft));
        }

        public void MoveCursorToEnd()
        {
            Console.ResetColor();
            SetCursorForPlayerState(0, 2);
        }

        private void SetCursorForPlayerState(int x, int y) =>
            Console.SetCursorPosition(x, state!.Map.Height + 1 + y);

        public void Uninitialise()
        {
            if (state == null) return;

            state.Player.HealthChanged -= Player_HealthChanged;
            foreach (var cell in state.Map.AllCells)
                cell.AppearanceChanged -= Cell_AppearanceChanged;

            state = null;
        }
    }
}
