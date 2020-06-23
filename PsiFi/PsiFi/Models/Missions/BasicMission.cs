using PsiFi.Engines.MapGenerators;
using PsiFi.Models.Mapping;
using PsiFi.Models.Mapping.Actors.Mobs;
using System.Collections.Generic;

namespace PsiFi.Models.Missions
{
    class BasicMission : IMission
    {
        /// <inheritdoc/>
        public IEnumerable<Map> GetMaps(Campaign campaign)
        {
            var mobs = new List<Mob> { campaign.Player };
            for (int i = 0; i < 10; i++)
                mobs.Add(new MaintenanceDrone());
            yield return new OpenSquare(campaign.Random, mobs).CreateMap();
        }
    }
}
