using PsiFi.Models.Mapping.Geometry;

namespace PsiFi.Views
{
    class MapViewResponse
    {
        public static MapViewResponse Move(Direction direction) => new MapViewResponse { Action = MapViewAction.Move, Direction = direction };
        public static readonly MapViewResponse Quit = new MapViewResponse { Action = MapViewAction.Quit };
        public static readonly MapViewResponse Wait = new MapViewResponse { Action = MapViewAction.Wait };

        /// <summary>
        /// The selected action.
        /// </summary>
        public MapViewAction Action { get; private set; }

        /// <summary>
        /// The direction to act in.
        /// </summary>
        public Direction Direction { get; private set; }

        /// <summary>
        /// The location of the target cell.
        /// </summary>
        public Location Location { get; private set; }
    }
}
