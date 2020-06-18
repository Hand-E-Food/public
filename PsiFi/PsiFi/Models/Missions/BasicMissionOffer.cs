using System;

namespace PsiFi.Models.Missions
{
    class BasicMissionOffer : IMissionOffer
    {
        public string Name => "Basic Mission";

        public IMission CreateMission() => new BasicMission();
    }
}
