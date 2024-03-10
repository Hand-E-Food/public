namespace Rogue.Mapping.Creation
{
    /// <summary>
    /// Creates a map following a particular strategy.
    /// </summary>
    /// <typeparam name="TMap">The type of the map to produce.</typeparam>
    /// <typeparam name="TCell">The type of cells in the map.</typeparam>
    public interface IMapCreationStrategy<TMap, TCell> where TMap : IMap<TCell> where TCell : ICell
    {
        /// <summary>
        /// Create a new map.
        /// </summary>
        /// <returns>A new map.</returns>
        TMap CreateMap();
    }
}
