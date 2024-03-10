using System;
using System.Collections.Generic;

namespace PatherySolver.Bot
{
    internal class BotEngine
    {
        private readonly int bot;
        private readonly Array2D<int> distances;
        private readonly Map map;
        private readonly bool[] teleporterUsed = new bool[Cell.In.Length];
        private readonly Queue<Location> walkQueue;

        private Location botLocation;

        public HeatMap HeatMap { get; private set; }
        public bool IsValid { get; private set; } = true;
        public int TotalMoves { get; private set; } = 0;

        /// <summary>
        /// Initialises a stateful engine that walks a bot through the map.
        /// </summary>
        /// <param name="map">The map to walk through.</param>
        /// <param name="bot">The bot to walk.</param>
        public BotEngine(Map map, int bot)
        {
            this.map = map;
            this.bot = bot;
            
            distances = new Array2D<int>(map.Width, map.Height);
            HeatMap = new HeatMap(map.Width, map.Height);
            walkQueue = new Queue<Location>(map.Width + map.Height);
        }

        /// <summary>
        /// Walks the bot through the map.
        /// </summary>
        public void Solve()
        {
            for (int t = 0; IsValid && t <= map.Checkpoints; t++)
                IsValid = WalkToCheckpoint(t < map.Checkpoints ? Cell.Checkpoint[t] : Cell.Exit);

            if (IsValid)
                map.Moves[bot] = TotalMoves;
        }

        /// <summary>
        /// Walks the shortest path to a cell contianing the <paramref name="checkpoint"/>.
        /// </summary>
        /// <param name="checkpoint">The checkpoint to walk to.</param>
        /// <returns>True if the bot walked to the checkpoint.
        /// False if the bot could not find a path to the checkpoint.</returns>
        private bool WalkToCheckpoint(char checkpoint)
        {
            InitialiseWalk(checkpoint);
            
            while (walkQueue.TryDequeue(out var location))
                CalculateDistanceAround(location);

            if (botLocation == default)
                botLocation = SelectBotLocation();

            return WalkBot();
        }

        /// <summary>
        /// Resets instance variables and prepares them for the specified checkpoint.
        /// </summary>
        /// <param name="checkpoint">The checkpoint to walk to.</param>
        private void InitialiseWalk(char checkpoint)
        {
            distances.Reset(int.MaxValue);
            foreach (var location in map.Find(checkpoint))
            {
                walkQueue.Enqueue(location);
                distances[location] = 0;
            }
        }

        /// <summary>
        /// Calculates the distance to the nearest checkpoint for the cells surrounding the specified location.
        /// </summary>
        /// <param name="origin">The cell around which to measure the distance to the nearest checkpoint.</param>
        private void CalculateDistanceAround(Location origin)
        {
            foreach (var direction in Direction.All)
            {
                Location step;
                var range = 0;
                
                do step = origin + direction * ++range;
                while (Cell.Ice == map[step]);

                if (!Cell.IsWalkable(map[step], bot))
                    continue;

                var distance = distances[origin] + range;
                if (distances[step] > distance)
                {
                    distances[step] = distance;
                    walkQueue.Enqueue(step);
                }
            }
        }

        /// <summary>
        /// Selects the bot's starting point closest to a checkpoint.
        /// </summary>
        /// <returns>The bot's starting point closest to a checkpoint.</returns>
        private Location SelectBotLocation()
        {
            var botLocations = map.Find(Cell.Start[bot]).GetEnumerator();
            if (!botLocations.MoveNext())
                throw new InvalidOperationException("Bot does not have a starting point.");

            var bestLocation = botLocations.Current;
            var bestDistance = distances[bestLocation];
            while(botLocations.MoveNext())
            {
                if (bestDistance > distances[botLocations.Current])
                {
                    bestLocation = botLocations.Current;
                    bestDistance = distances[bestLocation];
                }
            }
            return bestLocation;
        }

        /// <summary>
        /// Walks from the specified location to the nearest checkpoint.
        /// </summary>
        /// <param name="botLocation">The location to start from.</param>
        /// <returns>True if the bot walked to the checkpoint.
        /// False if the bot could not find a path to the checkpoint.</returns>
        private bool WalkBot()
        {
            Location step = default;
            var moves = 0;
            var distance = distances[botLocation];
            while (distance > 0)
            {
                if (distance == int.MaxValue)
                    return false;

                int range = default;
                foreach (var direction in Direction.All)
                {
                    range = 0;
                    do step = botLocation + direction * ++range;
                    while (Cell.Ice == map[step]);

                    if (distances[step] == distance - range)
                        break;
                }

                moves += range;
                HeatMap[step]++;
                if (Cell.IsIn(map[step], out var teleporter) && !teleporterUsed[teleporter])
                {
                    teleporterUsed[teleporter] = true;
                    botLocation = map.Outs[teleporter];
                }
                else
                {
                    botLocation = step;
                }

                distance = distances[botLocation];
            }

            TotalMoves += moves;
            return true;
        }
    }
}
