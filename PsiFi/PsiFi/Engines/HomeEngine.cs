using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Views;
using PsiFi.Views.ColorConsole;

namespace PsiFi.Engines
{
    class HomeEngine
    {
        private readonly Campaign campaign;
        private readonly IUserInterface<HomeMenuResponse> homeMenu;

        public HomeEngine(Campaign campaign, IUserInterface<HomeMenuResponse> homeMenu)
        {
            this.campaign = campaign;
            this.homeMenu = homeMenu;
        }

        public void Begin()
        {
            var isRunning = true;
            while (isRunning)
            {
                var response = homeMenu.GetInput();
                switch (response.Action)
                {
                    case HomeMenuAction.Quit:
                        isRunning = false;
                        break;

                    case HomeMenuAction.StartMission:
                        StartMission(response);
                        break;
                }
            }
        }

        private void StartMission(HomeMenuResponse response)
        {
            var mission = response.MissionOffer.CreateMission();
            foreach (var map in mission.GetMaps(campaign))
            {
                var mapView = new MapView();
                campaign.Player.MapView = mapView;
                var mapEngine = new MapEngine(map, mapView, campaign.Random);
                var reason = mapEngine.Begin();
                if (reason.Contains(MapEndReason.Quit))
                    break;
            }
        }
    }
}
