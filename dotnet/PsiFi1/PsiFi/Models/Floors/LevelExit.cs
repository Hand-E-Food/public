using System;

namespace PsiFi.Models.Floors
{
    class LevelExit : Floor
    {
        public LevelExit(ConsoleColor color = ConsoleColor.Blue) : base(new Appearance('<', color))
        { }
    }
}
