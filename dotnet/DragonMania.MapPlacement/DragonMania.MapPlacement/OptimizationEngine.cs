using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DragonMania.MapPlacement
{
    /// <summary>
    /// Optimizes the placement of buildings on the map.
    /// </summary>
    public class OptimizationEngine
    {
        /// <summary>
        /// The buildings yet to be placed with the highest priority on top.
        /// </summary>
        private readonly Stack<Building> _buildings;

        /// <summary>
        /// The cancellation token that signals optimization whould stop.
        /// </summary>
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// The map to optimize upon.
        /// </summary>
        private readonly Map _map;

        /// <summary>
        /// The best solution found so far.
        /// </summary>
        private Solution _solution = new Solution { Score = int.MaxValue };

        /// <summary>
        /// Raised when a new solution is found.
        /// </summary>
        public event Action<Solution> SolutionUpdated;

        /// <summary>
        /// Initialises a new instance of the <see cref="OptimizationEngine"/> class.
        /// </summary>
        /// <param name="map">The map to optimize upon.</param>
        /// <param name="buildings">The buildings yet to be placed with the highest priority on top.</param>
        /// <param name="cancellationToken">The cancellation token that signals optimization whould stop.</param>
        public OptimizationEngine(Map map, IEnumerable<Building> buildings, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (buildings == null)
                throw new ArgumentNullException(nameof(buildings));

            _cancellationToken = cancellationToken;
            _map = map;
            _buildings = new Stack<Building>(buildings.Reverse());
        }

        /// <summary>
        /// Optimizes the building placement on the map.
        /// </summary>
        public void Optimize()
        {
            UpdateSolution();
            var y = _map.MinY();
            var x = _map.MinX(y);
            if (Optimize(x, y))
                UpdateSolution();
        }

        private bool Optimize(int x, int y)
        {
            var building = _buildings.Pop();
            var size = building.Size;
            var maxY = _map.MaxY() - size;
            while (y <= maxY)
            {
                var maxX = _map.MaxX(y) - size;
                while (x <= maxX)
                {
                    if (_map.CanPlaceBuilding(x, y, size))
                    {
                        building.Left = x;
                        building.Top = y;
                        _map.PlaceBuilding(building);

                        bool optimized;
                        if (_buildings.Count == 0)
                            optimized = true;
                        else if (_buildings.Peek().Size == size)
                            optimized = Optimize(x + size, y);
                        else
                        {
                            var minY = _map.MinY();
                            var minX = _map.MinX(minY);
                            optimized = Optimize(minX, minY);
                        }
                        if (optimized)
                            return true;

                        _map.RemoveLastBuilding();
                    }

                    if (_cancellationToken.IsCancellationRequested)
                        break;

                    x++;
                }

                if (_cancellationToken.IsCancellationRequested)
                    break;

                y++;
                x = _map.MinX(y);
            }

            _buildings.Push(building);

            if (!building.Required)
                UpdateSolution();

            return false;
        }

        private void UpdateSolution()
        {
            var score = _buildings.Sum(x => x.Size * x.Size);
            if (score >= _solution.Score)
                return;

            _solution = new Solution
            {
                Map = _map.Clone(),
                RemainingBuildings = GetRemainingBuildingPools(),
                Score = score,
            };

            SolutionUpdated(_solution);
        }

        private IEnumerable<BuildingPool> GetRemainingBuildingPools()
        {
            return _buildings
                .GroupBy(x => x.Size)
                .OrderBy(x => x.Key)
                .Select(x => new BuildingPool { Size = x.Key, Count = x.Count(), Required = false });

        }
    }
}
