namespace Rogue.Mapping
{
    /// <summary>
    /// A cell on a map.
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// This cell's location on the map.
        /// </summary>
        Point Location { get; set; }
    }
}
