using System;

namespace RandomVectorMap.Generation
{

    /// <summary>
    /// A class that is randomized.
    /// </summary>
    public interface IRandomized
    {

        /// <summary>
        /// Sets the random number generator to use.
        /// </summary>
        Random Random { set; }
    }
}
