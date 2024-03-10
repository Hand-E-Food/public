using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Rogue.RogueMath;

namespace Rogue.Mapping
{
    public class FieldOfView<TCell> where TCell : ICell
    {
        private readonly HashSet<int> inFov;
        private readonly IMap<TCell> map;
        private readonly Rectangle mapBounds;
        private readonly int mapWidth;

        /// <summary>
        /// Constructs a new FieldOfView class for the specified Map
        /// </summary>
        /// <param name="map">The Map that this FieldOfView class will use to perform its field-of-view calculations</param>
        public FieldOfView(IMap<TCell> map)
        {
            this.map = map;
            mapBounds = new Rectangle(map.Size);
            mapWidth = map.Size.Width;
            inFov = new HashSet<int>();
        }

        private FieldOfView(IMap<TCell> map, IEnumerable<int> inFov)
        {
            this.map = map;
            mapWidth = map.Size.Width;
            this.inFov = new HashSet<int>(inFov);
        }

        /// <summary>
        /// Create and return a deep copy of an existing FieldOfView class
        /// </summary>
        /// <returns>A deep copy of an existing FieldOfViewClass</returns>
        public FieldOfView<TCell> Clone() => new FieldOfView<TCell>(map, inFov);

        /// <summary>
        /// Check if the Cell is in the currently computed field-of-view
        /// Field-of-view must first be calculated by calling ComputeFov and/or AppendFov
        /// </summary>
        /// <remarks>
        /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius
        /// </remarks>
        /// <example>
        /// Field-of-view can be used to simulate a character holding a light source and exploring a Map representing a dark cavern
        /// Any Cells within the FOV would be what the character could see from their current location and lighting conditions
        /// </example>
        /// <param name="location">The location of the Cell to check.</param>
        /// <returns>True if the Cell is in the currently computed field-of-view, false otherwise.</returns>
        public bool IsVisible(Point location) => IsVisible(location.X, location.Y);

        /// <summary>
        /// Check if the Cell is in the currently computed field-of-view
        /// Field-of-view must first be calculated by calling ComputeFov and/or AppendFov
        /// </summary>
        /// <remarks>
        /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius
        /// </remarks>
        /// <example>
        /// Field-of-view can be used to simulate a character holding a light source and exploring a Map representing a dark cavern
        /// Any Cells within the FOV would be what the character could see from their current location and lighting conditions
        /// </example>
        /// <param name="x">The x location of the Cell to check.</param>
        /// <param name="y">The y location of the Cell to check.</param>
        /// <returns>True if the Cell is in the currently computed field-of-view, false otherwise.</returns>
        public bool IsVisible(int x, int y) => inFov.Contains(IndexFor(x, y));

        /// <summary>
        /// Performs a field-of-view calculation with the specified parameters.
        /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius.
        /// Any existing field-of-view calculations will be overwritten when calling this method.
        /// </summary>
        /// <param name="origin">Origin location of the Cell from which to perform the field-of-view calculation.</param>
        /// <param name="radius">The number of Cells in which the field-of-view extends from the origin Cell. Think of this as the intensity of the light source.</param>
        /// <param name="includeWalls">True if walls should be included in the field-of-view when they are within the radius of the light source. False excludes walls even when they are within range.</param>
        /// <param name="isTransparent">A function that decides whether a cell is transparent.</param>
        /// <returns>List of Cells representing what is observable in the Map based on the specified parameters</returns>
        public ReadOnlyCollection<TCell> ComputeFov(Point origin, int radius, bool includeWalls, Func<TCell, bool> isTransparent)
        {
            ClearFov();
            return AppendFov(origin, radius, includeWalls, isTransparent);
        }

        /// <summary>
        /// Clears all cells from the POV cache.
        /// </summary>
        public void ClearFov() => inFov.Clear();

        /// <summary>
        /// Performs a field-of-view calculation with the specified parameters and appends it any existing field-of-view calculations.
        /// Field-of-view (FOV) is basically a calculation of what is observable in the Map from a given Cell with a given light radius.
        /// </summary>
        /// <example>
        /// When a character is holding a light source in a large area that also has several other sources of light such as torches along the walls
        /// ComputeFov could first be called for the character and then AppendFov could be called for each torch to give us the final combined FOV given all the light sources
        /// </example>
        /// <param name="origin">Origin location of the Cell from which to perform the field-of-view calculation.</param>
        /// <param name="radius">The number of Cells in which the field-of-view extends from the origin Cell. Think of this as the intensity of the light source.</param>
        /// <param name="includeWalls">True if walls should be included in the field-of-view when they are within the radius of the light source. False excludes walls even when they are within range.</param>
        /// <param name="isTransparent">A function that decides whether a cell is transparent.</param>
        /// <returns>List of Cells representing what is observable in the Map based on the specified parameters</returns>
        public ReadOnlyCollection<TCell> AppendFov(Point origin, int radius, bool includeWalls, Func<TCell, bool> isTransparent)
        {
            var radiusSquared = RadiusSquared(radius);
            var square = new Rectangle(origin, radius);
            square.Intersect(mapBounds);
            foreach (var borderPoint in square.AllPointsOnPerimiter)
            {
                foreach (var linePoint in Shape.GetPointsInLine(origin, borderPoint))
                {
                    if ((origin - linePoint).RadiusSquared > radiusSquared)
                        continue;

                    if (isTransparent(map[linePoint]))
                    {
                        inFov.Add(IndexFor(linePoint));
                    }
                    else
                    {
                        if (includeWalls)
                        {
                            inFov.Add(IndexFor(linePoint));
                        }
                        break;
                    }
                }
            }

            if (includeWalls)
            {
                // Post processing step created based on the algorithm at this website:
                // https://sites.google.com/site/jicenospam/visibilitydetermination
                foreach (var squarePoint in Shape.GetPointsInCircle(origin, radius, new Rectangle(map.Size)))
                {
                    if (squarePoint.X > origin.X)
                    {
                        if (squarePoint.Y > origin.Y)
                        {
                            PostProcessFovQuadrant(squarePoint, Quadrant.SE, isTransparent);
                        }
                        else if (squarePoint.Y < origin.Y)
                        {
                            PostProcessFovQuadrant(squarePoint, Quadrant.NE, isTransparent);
                        }
                    }
                    else if (squarePoint.X < origin.X)
                    {
                        if (squarePoint.Y > origin.Y)
                        {
                            PostProcessFovQuadrant(squarePoint, Quadrant.SW, isTransparent);
                        }
                        else if (squarePoint.Y < origin.Y)
                        {
                            PostProcessFovQuadrant(squarePoint, Quadrant.NW, isTransparent);
                        }
                    }
                }
            }

            return CellsInFov();
        }

        private ReadOnlyCollection<TCell> CellsInFov()
        {
            var cells = new List<TCell>(inFov.Count);
            foreach (int index in inFov)
                cells.Add(map[index % mapWidth, index / mapWidth]);
            return new ReadOnlyCollection<TCell>(cells);
        }

        private void PostProcessFovQuadrant(Point location, Quadrant quadrant, Func<TCell, bool> isTransparent)
        {
            int x = location.X;
            int y = location.Y;
            int x1 = x;
            int y1 = y;
            int x2 = x;
            int y2 = y;
            switch (quadrant)
            {
                case Quadrant.NE:
                    y1 = y + 1;
                    x2 = x - 1;
                    break;

                case Quadrant.SE:
                    y1 = y - 1;
                    x2 = x - 1;
                    break;

                case Quadrant.SW:
                    y1 = y - 1;
                    x2 = x + 1;
                    break;

                case Quadrant.NW:
                    y1 = y + 1;
                    x2 = x + 1;
                    break;
            }
            if (!IsVisible(x, y) && !isTransparent(map[x, y]))
            {
                if ((isTransparent(map[x1, y1]) && IsVisible(x1, y1))
                    || (isTransparent(map[x2, y2]) && IsVisible(x2, y2))
                    || (isTransparent(map[x2, y1]) && IsVisible(x2, y1)))
                {
                    inFov.Add(IndexFor(x, y));
                }
            }
        }

        private int IndexFor(Point location) => IndexFor(location.X, location.Y);

        private int IndexFor(int x, int y) => x + y * mapWidth;

        private enum Quadrant
        {
            NE = 1,
            SE = 2,
            SW = 3,
            NW = 4
        }
    }
}
