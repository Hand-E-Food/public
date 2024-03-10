using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonMania.MapPlacement
{
    public class Map : ICloneable
    {

        private List<Building> _placedBuildings;

        private char[][] _grid;

        public bool HasPlacedBuildings => _placedBuildings.Count > 0;

        private Map(Map clone)
        {
            _placedBuildings = clone._placedBuildings.Select(x => x.Clone()).ToList();
            _grid = clone._grid.Select(x => x.ToArray()).ToArray();
        }

        public Map(string[] grid)
        {
            _placedBuildings = new List<Building>();
            _grid = grid.Select(x => x?.ToCharArray() ?? new char[0]).ToArray();
        }

        private char this[int x, int y]
        {
            get
            {
                if (y < 0 || y >= _grid.Length)
                    return ' ';

                var row = _grid[y];
                if (x < 0 || x >= row.Length)
                    return ' ';

                return row[x];
            }
            set
            {
                if (y < 0 || y >= _grid.Length)
                    return;

                var row = _grid[y];
                if (x < 0 || x >= row.Length)
                    return;

                row[x] = value;
            }
        }

        /// <summary>
        /// Checks whether a building can be placed at the specified coordinates.
        /// </summary>
        /// <param name="left">The building's left coordinate.</param>
        /// <param name="top">The building's top coordinate.</param>
        /// <param name="size">The building's size.</param>
        /// <returns>True if the building can be placed here; otherwise, false.</returns>
        public bool CanPlaceBuilding(int left, int top, int size) =>
            Enumerable.Range(left, size).All(x => Enumerable.Range(top, size).All(y => this[x, y] == '.'));

        /// <summary>
        /// Returns a deep clone of this object.
        /// </summary>
        /// <returns>A deep clone of this object.</returns>
        public Map Clone() => new Map(this);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// Gets the maximum X coordinate.
        /// </summary>
        /// <param name="y">The y coordinate for which to get the maximum X coordinate.</param>
        /// <returns>The maximum X coordinate for the specified Y coordinate.</returns>
        public int MaxX(int y) => y >= 0 && y < _grid.Length ? _grid[y].Length : 0;

        /// <summary>
        /// Gets the maximum Y coordinate.
        /// </summary>
        /// <returns>The maximum Y coordinate.</returns>
        public int MaxY() => _grid.Length;

        /// <summary>
        /// Gets the minimum X coordinate.
        /// </summary>
        /// <param name="y">The y coordinate for which to get the minimum X coordinate.</param>
        /// <returns>The minimum X coordinate for the specified Y coordinate.</returns>
        public int MinX(int y) => y >= 0 && y < _grid.Length ? _grid[y].TakeWhile(x => x != '.').Count() : 0;

        /// <summary>
        /// Gets the minimum Y coordinate.
        /// </summary>
        /// <returns>The minimum Y coordinate.</returns>
        public int MinY() => _grid.TakeWhile(y => y.All(x => x != '.')).Count();

        /// <summary>
        /// Places a building on the map.
        /// </summary>
        /// <param name="building">The building to place.</param>
        /// <exception cref="InvalidOperationException">A building of that size cannot be placed there.</exception>
        public void PlaceBuilding(Building building)
        {
            if (!CanPlaceBuilding(building.Left, building.Top, building.Size))
                throw new InvalidOperationException("A building of that size cannot be placed there.");

            _placedBuildings.Add(building);
            this[building.Left , building.Top   ] = '┌';
            this[building.Right, building.Top   ] = '┐';
            this[building.Left , building.Bottom] = '└';
            this[building.Right, building.Bottom] = '┘';
            for (int x = building.Left + 1; x < building.Right; x++)
            {
                this[x, building.Top   ] = '─';
                this[x, building.Bottom] = '─';
            }
            for (int y = building.Top + 1; y < building.Bottom; y++)
            {
                this[building.Left , y] = '│';
                this[building.Right, y] = '│';
                for (int x = building.Left + 1; x < building.Right; x++)
                    this[x, y] = (char)(48 + building.Size);
            }
        }

        /// <summary>
        /// Removes the last placed building from this map.
        /// </summary>
        /// <returns>The removed building.</returns>
        /// <exception cref="InvalidOperationException">There are no buildings on this map.</exception>
        public Building RemoveLastBuilding()
        {
            if (_placedBuildings.Count == 0)
                throw new InvalidOperationException("The map has no buildings.");

            var last = _placedBuildings.Count - 1;
            var building = _placedBuildings[last];
            _placedBuildings.RemoveAt(last);

            for (int y = building.Top; y <= building.Bottom; y++)
                for (int x = building.Left; x <= building.Right; x++)
                    this[x, y] = '.';

            return building;
        }

        internal IEnumerable<string> ToLines() => _grid.Select(x => new string(x));

        public override string ToString() => string.Join(Environment.NewLine, ToLines());
    }
}
