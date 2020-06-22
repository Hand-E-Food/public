using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Models.Mapping
{
    class MapEndReasons
    {
        private readonly MapEndReason[] endReasons;

        public MapEndReasons(IEnumerable<MapEndReason> endReasons)
        {
            this.endReasons = endReasons
                .Where(endReason => endReason != null)
                .ToArray();
        }

        public bool None => endReasons.Length == 0;

        public bool Contains(MapEndReason reason) => endReasons.Contains(reason);
    }
}
