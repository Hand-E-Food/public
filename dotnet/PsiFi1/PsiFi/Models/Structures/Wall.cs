using System;

namespace PsiFi.Models.Structures
{
    /// <summary>
    /// A solid wall.
    /// </summary>
    class Wall : Structure
    {
        /// <summary>
        /// An indestructable wall.
        /// </summary>
        public static readonly Wall Indestructable = new IndestructableWall();

        /// <inheritdoc/>
        public override bool BlocksEnergy => true;

        /// <inheritdoc/>
        public override bool BlocksProjectiles => true;

        /// <summary>
        /// Initialises a <see cref="Wall"/> with the default appearance.
        /// </summary>
        public Wall() : base(new Appearance('#', ConsoleColor.Gray, ConsoleColor.DarkGray))
        { }

        /// <summary>
        /// Initialises a <see cref="Wall"/>.
        /// </summary>
        /// <param name="appearance">This wall's appearance.</param>
        public Wall(Appearance appearance) : base(appearance)
        { }
    }
}
