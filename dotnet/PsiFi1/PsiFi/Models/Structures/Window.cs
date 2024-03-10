using System;

namespace PsiFi.Models.Structures
{
    /// <summary>
    /// A transparent window.
    /// </summary>
    class Window : Structure
    {
        /// <summary>
        /// An indestructable window.
        /// </summary>
        public static readonly Window Indestructable = new IndestructableWindow();

        /// <inheritdoc/>
        public override bool BlocksEnergy => false;

        /// <inheritdoc/>
        public override bool BlocksProjectiles => true;

        /// <summary>
        /// Initialises a <see cref="Window"/> with the default appearance.
        /// </summary>
        public Window() : base(new Appearance('#', ConsoleColor.Cyan, ConsoleColor.DarkGray))
        { }

        /// <summary>
        /// Initialises a <see cref="Window"/>.
        /// </summary>
        /// <param name="appearance">This window's appearance.</param>
        public Window(Appearance appearance) : base(appearance)
        { }
    }
}
