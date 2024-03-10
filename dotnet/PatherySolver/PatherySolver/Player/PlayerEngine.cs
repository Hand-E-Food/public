using PatherySolver.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PatherySolver.Player
{
    public class PlayerEngine
    {
        /// <summary>
        /// The cancellation token that can stop optimization.
        /// </summary>
        private CancellationToken cancellationToken;

        /// <summary>
        /// The outcomes not yet fully processed.
        /// </summary>
        private List<Outcome> candidateOutcomes = new List<Outcome>();

        /// <summary>
        /// The states already known.
        /// </summary>
        private HashSet<Id> knownIds = new HashSet<Id>();

        /// <summary>
        /// True if optimization is cancelled. Otherwise, false.
        /// </summary>
        private bool IsCancelled => cancellationToken.IsCancellationRequested;

        /// <summary>
        /// The most optimal solution found for the map.
        /// </summary>
        public Map OptimalSolution
        {
            get => optimalSolution;
            private set
            {
                optimalSolution = value;
                OptimalSolutionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private Map optimalSolution;
        public event EventHandler OptimalSolutionChanged;

        /// <summary>
        /// A sample of the current optimization process. Thread-safe.
        /// </summary>
        public Map Sample
        {
            get
            {
                bool lockTaken = false;
                sampleLock.Enter(ref lockTaken);
                if (!lockTaken) return null;
                var value = sample;
                sampleLock.Exit();
                return value;
            }
            private set
            {
                bool lockTaken = false;
                while (!lockTaken)
                    sampleLock.Enter(ref lockTaken);
                sample = value;
                sampleLock.Exit();
            }
        }
        private Map sample = null;
        private SpinLock sampleLock = new();

        /// <summary>
        /// Initialises a new instance of the <see cref="PlayerEngine">.</see>
        /// </summary>
        /// <param name="map">The map to optimize.</param>
        public PlayerEngine(Map map)
        {
            OptimalSolution = map;
        }

        /// <summary>
        /// Optimizes the map to make the longest path.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can stop this asynchronous process.</param>
        public async Task Optimize(CancellationToken cancellationToken = default)
        {
            this.cancellationToken = cancellationToken;
            candidateOutcomes.Add(await Walk(OptimalSolution).ConfigureAwait(false));
            while (!IsCancelled && candidateOutcomes.Count > 0)
            {
                var bestOutcomes = GetBestOutcomes();
                var tasks = new List<Task<Outcome[]>>(bestOutcomes.Count);
                foreach (var outcome in bestOutcomes)
                {
                    if (CanExpandFrom(outcome, out var hottestLocations))
                        tasks.Add(ExpandFrom(outcome, hottestLocations));
                    else
                        candidateOutcomes.Remove(outcome);
                }

                AddCandidates(await Task.WhenAll(tasks));
            }
        }

        private void AddCandidates(Outcome[][] newOutcomes)
        {
            candidateOutcomes.AddRange(newOutcomes
                .SelectMany(outcomes => outcomes)
                .Where(outcome => outcome != null)
            );
            candidateOutcomes.Sort();
            Sample = candidateOutcomes.LastOrDefault()?.Map;
        }

        /// <summary>
        /// Gets all candidate outcomes that share the same highest score.
        /// </summary>
        /// <returns>All candidate outcomes that share the same highest score.</returns>
        private List<Outcome> GetBestOutcomes()
        {
            var i = candidateOutcomes.Count - 1;
            var bestOutcome = candidateOutcomes[i];
            if (OptimalSolution.TotalMoves < bestOutcome.Map.TotalMoves)
                OptimalSolution = bestOutcome.Map;

            var count = 1;
            while (--i >= 0 && candidateOutcomes[i].CompareTo(bestOutcome) == 0)
                count++;
            i++;
            var bestOutcomes = candidateOutcomes.GetRange(i, count);
            return bestOutcomes;
        }

        /// <summary>
        /// Verifies if the specified outcome can have walls added to it.
        /// </summary>
        /// <param name="outcome">The outcome to verify.</param>
        /// <param name="hottestLocations">Returns the hottest locations to try to add walls to.</param>
        /// <returns>True if the outcome can have walls added to it.
        /// False if the user has no more walls to add, or all useful locations have been previously attempted.</returns>
        private bool CanExpandFrom(Outcome outcome, out List<Location> hottestLocations)
        {
            if (outcome.Map.Walls > 0)
                hottestLocations = outcome.HeatMap.PopHottestLocations();
            else
                hottestLocations = new List<Location>(0);

            return hottestLocations.Count > 0;
        }

        /// <summary>
        /// Optimizes from a previous outcome.
        /// </summary>
        /// <param name="outcome">The outcome to optimize from.</param>
        private Task<Outcome[]> ExpandFrom(Outcome outcome, IEnumerable<Location> hottestLocations)
        {
            var tasks = hottestLocations
                .Where(location => Cell.Empty == outcome.Map[location])
                .Select(location => AddWall(outcome.Map, location))
                .Where(map => knownIds.Add(new Id(map)))
                .Select(Walk);
            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// Expands upon the map by adding a wall to the specified location.
        /// </summary>
        /// <param name="map">The map to expand upon.</param>
        /// <param name="location">The location to add a wall at.</param>
        /// <returns>The map with a wall added.</returns>
        private Map AddWall(Map map, Location location)
        {
            map = map.Clone();
            map.AddWall(location);
            return map;
        }

        /// <summary>
        /// Walks the specified map.
        /// </summary>
        /// <param name="map">The map to walk.</param>
        /// <returns>The outcome of walking the map.
        /// Null if the map cannot be walked or optimization is cancelled.</returns>
        private async Task<Outcome> Walk(Map map)
        {
            if (IsCancelled) return null;
            var botsEngine = new BotsEngine(map);
            await botsEngine.Walk().ConfigureAwait(false);
            return botsEngine.IsValid ? new(botsEngine) : null;
        }
    }
}
