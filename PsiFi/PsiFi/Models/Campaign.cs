using PsiFi.Engines;
using PsiFi.Models.Mapping.Actors.Mobs;
using PsiFi.Models.Missions;
using System.Collections.Generic;

namespace PsiFi.Models
{
    /// <summary>
    /// Represents the state of a campaign.
    /// </summary>
    class Campaign
    {
        /// <summary>
        /// The current;y available mission offers.
        /// </summary>
        public List<IMissionOffer> MissionOffers { get; } = new List<IMissionOffer>();
        
        /// <summary>
        /// The player's mob.
        /// </summary>
        public Player Player { get; } = new Player();

        /// <summary>
        /// The random number generator.
        /// </summary>
        public IRandom Random { get; } = new Random();

        /// <summary>
        /// Initialises a new instance of the <see cref="Campaign"/>.
        /// </summary>
        public Campaign()
        {
            MissionOffers.Add(new BasicMissionOffer());
        }
    }
}
