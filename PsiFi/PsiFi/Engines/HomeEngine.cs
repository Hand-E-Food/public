using PsiFi.Models;
using PsiFi.Views;
using PsiFi.Views.ColorConsole;

namespace PsiFi.Engines
{
    class HomeEngine
    {
        private readonly World world;
        private readonly IUserInterface<HomeMenuResponse> homeMenu;

        public HomeEngine(World world, IUserInterface<HomeMenuResponse> homeMenu)
        {
            this.world = world;
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
            foreach (var map in mission.GetMaps(world))
            {
                var mapView = new MapView();
                world.Player.MapView = mapView;
                var mapEngine = new MapEngine(map, mapView);
                mapEngine.Begin();
            }
        }
    }
}
