using MarkRichardson.Battleship.Navy;

namespace MarkRichardson.Battleship.Gunners
{
    /// <summary>
    /// Represents an intelligent gunner.
    /// </summary>
    public interface IGunner
    {
        /// <summary>
        /// Selects a location on the battlefield and attacks it.
        /// </summary>
        /// <returns>The result of the attack.</returns>
        BattlefieldStatus Fire();
    }
}
