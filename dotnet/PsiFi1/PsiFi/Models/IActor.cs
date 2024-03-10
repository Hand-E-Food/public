using System.Collections.Generic;

namespace PsiFi.Models
{
    delegate void ActorEventHandler(IActor actor);

    interface IActor
    {
        /// <summary>
        /// The next time this actor can act.
        /// </summary>
        int ActTime { get; set; }
        /// <summary>
        /// Raised after this actor's <see cref="ActTime"/> has changed.
        /// </summary>
        event ActorEventHandler? ActTimeChanged;

        /// <summary>
        /// This actor's brain. Null if player input is required.
        /// </summary>
        IBrain? Brain { get; }

        /// <summary>
        /// Gets any child actors this actor has.
        /// </summary>
        /// <returns>Child actors of this actor.</returns>
        IEnumerable<IActor> GetChildActors();
    }
}
