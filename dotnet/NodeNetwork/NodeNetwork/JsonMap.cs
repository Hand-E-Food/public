using System.Collections.Generic;

namespace NodeNetwork
{
    internal class JsonMap
    {
        public List<JsonNode> Nodes { get; set; } = new List<JsonNode>();
        public List<JsonBridge> Bridges { get; set; } = new List<JsonBridge>();
    }
}
