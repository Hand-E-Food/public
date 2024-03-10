using PatherySolver.Bot;
using System;
using System.Linq;

namespace PatherySolver.Player
{
    /// <summary>
    /// Represents the outcome of bots walking a map.
    /// </summary>
    internal class Outcome : IComparable<Outcome>
    {
        /// <summary>
        /// This outcome's heat map.
        /// </summary>
        public HeatMap HeatMap { get; }

        /// <summary>
        /// This outcome's map.
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Outcome"/> class populated from a
        /// completed <see cref="BotsEngine"/>.
        /// </summary>
        /// <param name="botsEngine">The <see cref="BotsEngine"/> containing the outcome.</param>
        public Outcome(BotsEngine botsEngine)
        {
            HeatMap = botsEngine.GetHeatMap();
            Map = botsEngine.Map;
        }

        /// <inheritdoc/>
        public int CompareTo(Outcome other)
        {
            int result = Map.TotalMoves - other.Map.TotalMoves;
            if (result != 0) return result;
            result = Map.Walls - other.Map.Walls;
            return result;
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Map.TotalMoves} moves, {Map.Walls} walls yet to place";
    }
}
