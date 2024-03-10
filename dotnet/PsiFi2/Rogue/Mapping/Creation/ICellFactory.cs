namespace Rogue.Mapping.Creation
{
    /// <summary>
    /// Creates cells of a specific type for generic purposes.
    /// </summary>
    /// <typeparam name="TCell">The type of cells in the map.</typeparam>
    public interface ICellFactory<TCell> where TCell : ICell
    {
        /// <summary>
        /// Creates a cell that is walkable space.
        /// </summary>
        /// <returns>A cell that is walkable space.</returns>
        TCell CreateSpace();

        /// <summary>
        /// Creates a cell that is a solid wall.
        /// </summary>
        /// <returns>A cell that is a solid wall.</returns>
        TCell CreateWall();
    }
}
