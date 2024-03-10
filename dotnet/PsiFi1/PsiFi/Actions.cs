using PsiFi.Geometry;
using PsiFi.Models;

namespace PsiFi
{
    static class Actions
    {
        public static void Wait(Map map, IActor actor)
        {
            var actors = map.Actors;
            if (actors.Count > 1)
            {
                actors.Remove(actor);
                var actTime = map.Actors.First.ActTime;
                if (actTime == actor.ActTime) actTime++;
                actor.ActTime = actTime;
                map.Actors.Add(actor);
            }
        }

        public static bool Walk(Map map, Mob mob, Vector direction) => Walk(mob, map[mob.Cell!.Location + direction]);
        public static bool Walk(Mob mob, Cell targetCell)
        {
            if (targetCell.Occupant != null) return false;

            mob.Cell = targetCell;
            return true;
        }
    }
}
