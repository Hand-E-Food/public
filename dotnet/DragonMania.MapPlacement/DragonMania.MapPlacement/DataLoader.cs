using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DragonMania.MapPlacement
{
    public class DataLoader
    {
        public IEnumerable<Building> GetBuildings() =>
            JsonConvert.DeserializeObject<List<BuildingPool>>(File.ReadAllText("Buildings.json")).SelectMany(x => x.PopAll());

        public Map GetMap() =>
            new Map(File.ReadAllLines("Map.txt"));
    }
}
