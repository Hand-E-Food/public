using SpaceTraders.Api;

namespace SpaceTraders.Cli.DataModel
{
    internal class GlobalStateDto
    {
        public ICollection<Faction> Factions { get; set; }
        public DateTime ResetDate { get; set; }
        public ICollection<StarSystem> Systems { get; set; }
    }
}
