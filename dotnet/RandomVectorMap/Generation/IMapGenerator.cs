using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// A map generator component.
    /// </summary>
    public interface IMapGenerator : IStepper, IRandomized
    {

        /// <summary>
        /// Sets the map being generated.
        /// </summary>
        Map Map { set; }
    }
}
