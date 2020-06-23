using PsiFi.Models;
using PsiFi.Models.Mapping;
using PsiFi.Views;
using System.Linq;

namespace PsiFi.Engines
{
    class MapEngine
    {
        private readonly Map map;
        private readonly IMapView mapView;
        private readonly IRandom random;

        public MapEngine(Map map, IMapView mapView, IRandom random)
        {
            this.map = map;
            this.mapView = mapView;
            this.random = random;
        }

        public MapEndReasons Begin()
        {
            mapView.Map = map;
            MapEndReasons endReasons;
            do
            {
                var mapInterface = new MapInterface(map, map.Actors.Next, random);
                mapInterface.Interact();
                if (mapInterface.IsQuitting)
                {
                    endReasons = MapEndReasons.Quit;
                }
                else
                {
                    mapView.Update();
                    endReasons = new MapEndReasons(map.EndConditions.Select(condition => condition()));
                }
            }
            while (endReasons.None);
            return endReasons;
        }
    }
}
