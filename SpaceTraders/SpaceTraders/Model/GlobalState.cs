using SpaceTraders.Api;

namespace SpaceTraders.Model
{
    public class GlobalState
    {
        public Dictionary<FactionSymbols, Faction> Factions { get; set; }
        public DateTime ResetDate { get; set; }
        public Dictionary<string, Sector> Sectors { get; set; }
        public Dictionary<string, StarSystem> Systems { get; set; }
    }
}
