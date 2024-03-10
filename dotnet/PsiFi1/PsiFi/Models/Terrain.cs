using System;

namespace PsiFi.Models
{
    static class Terrain
    {
        /// <summary>
        /// The default floor.
        /// </summary>
        public static readonly Floor DefaultFloor = new Floor(new Appearance('·', ConsoleColor.Gray));
    }
}
