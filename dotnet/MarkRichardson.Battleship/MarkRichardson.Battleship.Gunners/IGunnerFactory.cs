using MarkRichardson.Battleship.Navy;

namespace MarkRichardson.Battleship.Gunners
{

    /// <summary>
    /// Provides methods for creating gunners of a particular class.
    /// </summary>
    public interface IGunnerFactory
    {

        /// <summary>
        /// Creates a new instance of a <see cref="IGunner"/> object.
        /// </summary>
        /// <param name="battlefield">The battlefield the gunner is aiming at.</param>
        /// <returns>The new gunner.</returns>
        IGunner CreateGunner(Battlefield battlefield);

        /// <summary>
        /// This gunner's name.
        /// </summary>
        string Name { get; }
    }
}
