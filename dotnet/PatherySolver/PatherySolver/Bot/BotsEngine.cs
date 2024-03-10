using System.Linq;
using System.Threading.Tasks;

namespace PatherySolver.Bot
{
    /// <summary>
    /// An single-use, stateful engine for guiding the bots along the shortest route to the checkpoints and exit.
    /// </summary>
    public class BotsEngine
    {
        private readonly BotEngine[] botEngines;

        /// <summary>
        /// True if the map can be walked to completion by all bots.
        /// False if one of more bots cannot reach all checkpoints and the exit.
        /// </summary>
        public bool IsValid => botEngines.All(botEngine => botEngine.IsValid);

        /// <summary>
        /// The map this <see cref="BotsEngine"/> walks.
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="BotsEngine"/> class.
        /// </summary>
        /// <param name="map">The map to walk.</param>
        public BotsEngine(Map map)
        {
            Map = map;

            botEngines = Enumerable
                .Range(0, map.Bots)
                .Select(bot => new BotEngine(map, bot))
                .ToArray();
        }

        /// <summary>
        /// Gets the heat map generated while walking the map.
        /// </summary>
        /// <returns>The heat map generated while walking the map.</returns>
        public HeatMap GetHeatMap()
        {
            var height = Map.Height;
            var width = Map.Width;

            var result = botEngines[0].HeatMap.Clone();
            foreach (var heatMap in botEngines.Skip(1).Select(botEngine => botEngine.HeatMap))
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        result[x, y] += heatMap[x, y];

            return result;
        }

        /// <summary>
        /// Walks all bots to each checkpoint and the exit.
        /// </summary>
        public Task Walk() => Task.WhenAll(botEngines.Select(botEngine => Task.Run(botEngine.Solve)));
    }
}
