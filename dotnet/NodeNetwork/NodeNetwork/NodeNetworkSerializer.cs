using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NodeNetwork
{
    /// <summary>
    /// Manages serialization of NodeNetwork objects.
    /// </summary>
    public static class NodeNetworkSerializer
    {
        /// <summary>
        /// Serializes the <paramref name="map"/> into a JSON <see cref="string"/>.
        /// </summary>
        /// <param name="map">The <see cref="Map"/> to serialize.</param>
        /// <returns>The JSON representation of the <paramref name="map"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="map"/> is null.</exception>
        public static string Serialize(Map map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            JsonMap jsonMap = ToJson(map);
            string json = JsonConvert.SerializeObject(jsonMap);
            return json;
        }

        private static JsonMap ToJson(Map map)
        {
            JsonMap jsonMap = new JsonMap();

            Dictionary<Node, int> nodeIds = new Dictionary<Node, int>();
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                Node node = map.Nodes[i];
                nodeIds.Add(node, i);
                JsonNode jsonNode = new JsonNode {
                    Id = i,
                    Label = node.Label,
                    X = node.Location.X,
                    Y = node.Location.Y,
                };
                jsonMap.Nodes.Add(jsonNode);
            }

            foreach (Bridge bridge in map.Bridges)
            {
                JsonBridge jsonBridge = new JsonBridge {
                    Label = bridge.Label,
                    NodeIds = bridge.Nodes.Select(node => nodeIds[node]).ToArray(),
                };
                jsonMap.Bridges.Add(jsonBridge);
            }

            return jsonMap;
        }

        /// <summary>
        /// Deserializes the JSON <see cref="string"/> into a <see cref="Map"/> object.
        /// </summary>
        /// <param name="json">The JSON <see cref="string"/> to deserialize.</param>
        /// <returns>The <see cref="Map"/> represented by the <paramref name="json"/> stirng.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="json"/> is null.</exception>
        public static Map Deserialize(string json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            JsonMap jsonMap = JsonConvert.DeserializeObject<JsonMap>(json);
            Map map = FromJson(jsonMap);
            return map;
        }

        private static Map FromJson(JsonMap jsonMap)
        {
            Map map = new Map();

            Dictionary<int, Node> nodes = new Dictionary<int, Node>();
            foreach (JsonNode jsonNode in jsonMap.Nodes)
            {
                Node node = new Node {
                    Label = jsonNode.Label,
                    Location = new PointF(jsonNode.X, jsonNode.Y),
                };
                map.Nodes.Add(node);
                nodes.Add(jsonNode.Id, node);
            }

            foreach (JsonBridge jsonBridge in jsonMap.Bridges)
            {
                Bridge bridge = new Bridge(jsonBridge.NodeIds.Select(i => nodes[i])) {
                    Label = jsonBridge.Label,
                };
            }

            return map;
        }
    }
}
