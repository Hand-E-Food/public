using PsiFi.Engines;
using System;

namespace PsiFi.Models.Mapping.Actors.Mobs
{
    class Player : IMob
    {
        public Appearance Appearance { get; } = new Appearance('@', ConsoleColor.White);

        public Cell Cell { get; set; }

        public Range Health { get; } = new Range(0, 100);

        public int NextTimeIndex { get; set; } = 0;

        public Player()
        { }

        public void Interact(MapInterface mapInterface)
        {
            throw new NotImplementedException();
        }
    }
}
