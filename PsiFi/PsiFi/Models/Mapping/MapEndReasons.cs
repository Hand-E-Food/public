using System.Collections.Generic;
using System.Linq;

namespace PsiFi.Models.Mapping
{
    class MapEndReasons
    {
        private readonly object[] endReasons;

        public MapEndReasons(IEnumerable<object> endReasons)
        {
            this.endReasons = endReasons
                .Where(endReason => endReason != null)
                .ToArray();
        }

        public bool None => endReasons.Length == 0;

        public bool Contains(object reason) => endReasons.Contains(reason);
    }
}
