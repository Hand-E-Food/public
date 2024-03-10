using PsiFi.Models;

namespace PsiFi.StateMachine
{
    class SelectActorAction : IStateMachineNode
    {
        private readonly SelectNextActor SelectNextActor = null!;

        /// <inheritdoc/>
        public IStateMachineNode? Execute(State state)
        {
            var actor = state.Map.Actors.First;
            actor.Brain!.Act(state, actor);
            return SelectNextActor;
        }
    }
}