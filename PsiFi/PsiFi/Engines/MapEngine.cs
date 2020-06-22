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

        public MapEngine(Map map, IMapView mapView)
        {
            this.map = map;
            this.mapView = mapView;
        }

        public MapEndReasons Begin()
        {
            mapView.Map = map;
            MapEndReasons endReasons;
            do
            {
                new MapInterface(map, map.Actors.Next).Interact();
                mapView.Update();
                endReasons = new MapEndReasons(map.EndConditions.Select(condition => condition()));
            }
            while (endReasons.None);
            return endReasons;
        }
    }
}
