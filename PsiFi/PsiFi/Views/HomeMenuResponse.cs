using PsiFi.Models;
using System;

namespace PsiFi.Views
{
    class HomeMenuResponse
    {
        public HomeMenuAction Action { get; private set; }
        
        public IMissionOffer MissionOffer { get; private set; }

        private HomeMenuResponse() { }

        public static readonly HomeMenuResponse Quit = new HomeMenuResponse
        {
            Action = HomeMenuAction.Quit,
        };

        public static HomeMenuResponse StartMission(IMissionOffer missionOffer) => new HomeMenuResponse
        {
            Action = HomeMenuAction.StartMission,
            MissionOffer = missionOffer,
        };
    }
}