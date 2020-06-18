using PsiFi.Models;
using PsiFi.Views;
using System;

namespace PsiFi.Engines
{
    class MapEngine
    {
        private Map map;
        private IMapView mapView;

        public MapEngine(Map map, IMapView mapView)
        {
            this.map = map;
            this.mapView = mapView;
        }

        internal void Begin()
        {
            mapView.Map = map;
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
        }
    }
}
