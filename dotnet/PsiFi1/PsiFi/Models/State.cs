using PsiFi.Generators.Maps;
using PsiFi.Models.Mobs;
using System.Collections.Generic;

namespace PsiFi.Models
{
    class State
    {
        /// <summary>
        /// The random number generator.
        /// </summary>
        public Random Random { get; } = new Random();

        /// <summary>
        /// The queue of maps to generate.
        /// </summary>
        public IEnumerator<IMapGenerator> MapGenerators { get; set; } = null!;

        /// <summary>
        /// The current map.
        /// </summary>
        public Map Map { get; set; } = null!;

        /// <summary>
        /// The player.
        /// </summary>
        public Player Player { get; set; } = null!;
    }
}
