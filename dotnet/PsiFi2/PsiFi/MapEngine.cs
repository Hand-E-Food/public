using PsiFi.ConsoleForms;
using PsiFi.Mapping;
using Rogue;
using System;
using System.Collections.Generic;

namespace PsiFi
{
    public class MapEngine
    {
        private readonly List<IActor> actors = new List<IActor>();

        public Map Map { get; set; } = null!;

        public MapScreen MapScreen { get; set; } = null!;

        public Player Player { get; set; } = null!;

        public void Initialize()
        {
            if (Map == null) throw new ArgumentNullException(nameof(Map));
            if (MapScreen == null) throw new ArgumentNullException(nameof(MapScreen));
            if (Player == null) throw new ArgumentNullException(nameof(Player));

            var playerEngine = new PlayerEngine();

            actors.Clear();
            actors.Add(Player);

            MapScreen.Health = Player.Health;
            MapScreen.Map = Map;
            MapScreen.MaximumHealth = Player.MaximumHealth;

            Player.HealthChanged += Player_HealthChanged;
            Player.MaximumHealthChanged += Player_MaximumHealthChanged;
            Player.PlayerEngine = playerEngine;

            playerEngine.MapScreen = MapScreen;
            playerEngine.Player = Player;

            playerEngine.Initialize();
        }

        private void Player_HealthChanged(object? sender, EventArgs e) =>
            MapScreen.Health = Player.Health;

        private void Player_MaximumHealthChanged(object? sender, EventArgs e) =>
            MapScreen.MaximumHealth = Player.MaximumHealth;

        public MapEngineResult Play()
        {
            MapScreen.Log("You enter the next level.");

            while(true)
            {
                var nextActor = actors.FirstWithLeast(actor => actor.TimeUntilNextTurn);
                var ticks = nextActor.TimeUntilNextTurn;
                if (ticks > 0)
                {
                    foreach (var actor in actors)
                        actor.PassTime(ticks);
                }
                var result = nextActor.Act();
                result.Perform(MapScreen);
            }
        }
    }
}
