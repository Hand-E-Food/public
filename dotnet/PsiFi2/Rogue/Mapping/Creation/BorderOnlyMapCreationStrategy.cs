namespace Rogue.Mapping.Creation
{
    /// <summary>
    /// Creates a map with only a border wall and empty space inside.
    /// </summary>
    /// <inheritdoc/>
    public class BorderOnlyMapCreationStrategy<TMap, TCell> : IMapCreationStrategy<TMap, TCell> where TMap : IMap<TCell>, new() where TCell : ICell
    {
        /// <summary>
        /// The factory for creating cells.
        /// </summary>
        protected ICellFactory<TCell> CellFactory { get; }

        /// <summary>
        /// The map's size.
        /// </summary>
        protected Size Size { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="BorderOnlyMapCreationStrategy"/> with the specified size.
        /// </summary>
        /// <param name="cellFactory">The factory for creating cells.</param>
        /// <param name="size">The map's size.</param>
        public BorderOnlyMapCreationStrategy(ICellFactory<TCell> cellFactory, Size size)
        {
            CellFactory = cellFactory;
            Size = size;
        }

        /// <inheritdoc/>
        public virtual TMap CreateMap()
        {
            var map = new TMap();
            map.Initialize(Size);

            int x, y;
            for (y = 1; y < map.Size.Height - 1; y++)
                for (x = 1; x < map.Size.Width - 1; x++)
                    map[x, y] = CellFactory.CreateSpace();

            y = map.Size.Height - 1;
            for (x = 0; x < map.Size.Width; x++)
            {
                map[x, 0] = CellFactory.CreateWall();
                map[x, y] = CellFactory.CreateWall();
            }
            x = map.Size.Width - 1;
            for (y = 1; y < map.Size.Height - 1; y++)
            {
                map[0, y] = CellFactory.CreateWall();
                map[x, y] = CellFactory.CreateWall();
            }

            return map;
        }
    }
}
