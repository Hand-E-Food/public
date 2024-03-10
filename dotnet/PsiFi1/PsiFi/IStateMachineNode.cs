using PsiFi.Models;

namespace PsiFi
{
    /// <summary>
    /// A node of the state machine.
    /// </summary>
    interface IStateMachineNode
    {
        /// <summary>
        /// Executes this node of the state machine.
        /// </summary>
        /// <param name="state">The state machine's current state.</param>
        /// <returns>The next <see cref="IStateMachineNode"/> to execute. Null to end the state machine.</returns>
        IStateMachineNode? Execute(State state);
    }
}
