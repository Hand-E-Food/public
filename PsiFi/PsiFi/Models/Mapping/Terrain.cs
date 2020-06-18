using PsiFi.Models.Mapping.Terrains;
using System;

namespace PsiFi.Models.Mapping
{
    class Terrain
    {
        public static Terrain Exit(string name) => new Exit
        {
            Appearance = new Appearance('<', ConsoleColor.Blue),
            AllowsMovement = true,
            AllowsProjectile = true,
            AllowsVision = true,
            Name = name,
        };

        public static readonly Terrain Floor = new Terrain
        {
            Appearance = new Appearance('.', ConsoleColor.Gray),
            AllowsMovement = true,
            AllowsProjectile = true,
            AllowsVision = true,
        };

        public static readonly Terrain Glass = new Terrain
        {
            Appearance = new Appearance('\'', ConsoleColor.Cyan),
            AllowsMovement = false,
            AllowsProjectile = false,
            AllowsVision = true,
        };

        public static readonly Terrain Hole = new Terrain
        {
            Appearance = new Appearance('~', ConsoleColor.DarkGray),
            AllowsMovement = false,
            AllowsProjectile = true,
            AllowsVision = true,
        };

        public static readonly Terrain Wall = new Terrain {
            Appearance = new Appearance('#', ConsoleColor.Gray),
            AllowsMovement = false,
            AllowsProjectile = false,
            AllowsVision = false,
        };

        public Appearance Appearance { get; protected set; }
        public bool AllowsMovement { get; protected set; }
        public bool AllowsProjectile { get; protected set; }
        public bool AllowsVision { get; protected set; }

        protected Terrain() { }
    }
}
