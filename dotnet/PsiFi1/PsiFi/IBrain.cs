using PsiFi.Models;

namespace PsiFi
{
    interface IBrain
    {
        /// <summary>
        /// Selects and performs an action upon the game state.
        /// </summary>
        /// <param name="state">The game's state.</param>
        void Act(State state);
    }
}
