using SpaceTraders.Api;

namespace SpaceTraders.Model
{
    public class Sector
    {
        public string Symbol { get; set; }
        public Dictionary<string, StarSystem> Systems { get; set; }
    }
}
