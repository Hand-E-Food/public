using PsiFi.Models;

namespace PsiFi.StateMachine
{
    class SelectNextActor : IStateMachineNode
    {
        private readonly PlayerIsDead PlayerIsDead = null!;
        private readonly SelectActorAction SelectActorAction = null!;
        private readonly SelectPlayerAction SelectPlayerAction = null!;

        /// <inheritdoc/>
        public IStateMachineNode? Execute(State state)
        {
            if (state.Player.Health <= 0)
                return PlayerIsDead;

            var actor = state.Map.Actors.First;
            if (actor.Brain == null)
                return SelectPlayerAction;
            else
                return SelectActorAction;
        }
    }
}
