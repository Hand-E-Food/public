using SpaceTraders.Api;

#pragma warning disable CS8618 // Non-nullable property '{}' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

namespace SpaceTraders
{
    public class GameState
    {
        public Agent Agent { get; set; }

        public Contract Contract { get; set; }

        public Faction Faction { get; set; }

        public List<Ship> Ships { get; } = new();

        public string Token { get; set; }
    }
}
