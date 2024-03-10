using PsiFi.Models;

namespace PsiFi.Rules
{
    interface IEffect
    {
        /// <summary>
        /// Executes the effect upon the specified <paramref name="target"/>.
        /// </summary>
        /// <param name="target">This effect's target.</param>
        /// <param name="state">The game's current state.</param>
        void Execute(Cell target, State state);
    }
}