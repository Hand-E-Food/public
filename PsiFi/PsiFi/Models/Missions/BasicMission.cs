using PsiFi.Engines.MapGenerators;
using System.Collections.Generic;

namespace PsiFi.Models.Missions
{
    class BasicMission : IMission
    {
        public IEnumerable<Map> GetMaps()
        {
            yield return new OpenSquare().CreateMap();
        }
    }
}
