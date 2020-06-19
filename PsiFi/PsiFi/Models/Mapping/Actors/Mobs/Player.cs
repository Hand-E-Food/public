using PsiFi.Engines;
using System;

namespace PsiFi.Models.Mapping.Actors.Mobs
{
    class Player : Mob
    {
        public Player() : base(new Appearance('@', ConsoleColor.White), 100)
        { }

        public override void Interact(MapInterface mapInterface)
        {
            throw new NotImplementedException();
        }
    }
}
