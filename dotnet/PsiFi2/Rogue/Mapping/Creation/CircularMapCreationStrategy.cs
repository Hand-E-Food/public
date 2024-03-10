using System;

namespace Rogue.Mapping.Creation
{
    /// <summary>
    /// Creates a map with only a circular border wall and empty space inside.
    /// </summary>
    /// <inheritdoc/>
    public class CircularMapCreationStrategy<TMap, TCell> : IMapCreationStrategy<TMap, TCell> where TMap : IMap<TCell>, new() where TCell : ICell
    {
        /// <summary>
        /// The map's bounds.
        /// </summary>
        protected Rectangle Bounds { get; }

        /// <summary>
        /// The factory for creating cells.
        /// </summary>
        protected ICellFactory<TCell> CellFactory { get; }

        /// <summary>
        /// The map's center point.
        /// </summary>
        protected Point Center { get; }

        /// <summary>
        /// The circle's interior radius.
        /// </summary>
        protected int Radius { get; }

        /// <summary>
        /// The map's size.
        /// </summary>
        protected Size Size { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="BorderOnlyMapCreationStrategy"/> with the specified size.
        /// </summary>
        /// <param name="cellFactory">The factory for creating cells.</param>
        /// <param name="size">The map's size.</param>
        public CircularMapCreationStrategy(ICellFactory<TCell> cellFactory, Size size)
        {
            Bounds = new Rectangle(size);
            CellFactory = cellFactory;
            Center = size.Center;
            Radius = Math.Min(Center.X, Center.Y) - 1;
            Size = size;
        }

        /// <inheritdoc/>
        public virtual TMap CreateMap()
        {
            var map = new TMap();
            map.Initialize(Size);

            foreach (var location in Shape.GetPointsInCircle(Center, Radius))
                map[location] = CellFactory.CreateSpace();

            foreach (var location in Bounds.AllPoints)
                if (map[location] == null)
                    map[location] = CellFactory.CreateWall();

            return map;
        }
    }
}
