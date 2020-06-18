using PsiFi.Models.Mapping.Mobs;
using PsiFi.Models.Missions;
using System.Collections.Generic;

namespace PsiFi.Models
{
    class World
    {
        public List<IMissionOffer> MissionOffers { get; } = new List<IMissionOffer>();
        public Player Player { get; } = new Player();

        public World()
        {
            MissionOffers.Add(new BasicMissionOffer());
        }
    }
}
