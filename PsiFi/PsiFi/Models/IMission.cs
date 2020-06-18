using System.Collections.Generic;

namespace PsiFi.Models
{
    /// <summary>
    /// Manages a sequence of maps.
    /// </summary>
    interface IMission
    {
        /// <summary>
        /// Returns a sequence of maps.
        /// </summary>
        /// <returns>A sequence of maps.</returns>
        /// <remarks>Each map can be chosen and created at the time it is required, so prior decisions can affect future maps or even cause branching.</remarks>
        IEnumerable<Map> GetMaps();
    }
}
