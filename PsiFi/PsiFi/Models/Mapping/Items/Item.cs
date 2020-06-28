namespace PsiFi.Models.Mapping.Items
{
    /// <summary>
    /// An item that can be carried.
    /// </summary>
    abstract class Item
    {
        /// <summary>
        /// This item's appearance.
        /// </summary>
        public abstract Appearance Appearance { get; }

        /// <summary>
        /// This item's name.
        /// </summary>
        public abstract string Name { get; }
    }
}
