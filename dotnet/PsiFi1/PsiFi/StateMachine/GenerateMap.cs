using PsiFi.Models;

namespace PsiFi.StateMachine
{
    class GenerateMap : IStateMachineNode
    {
        private readonly EnterMap EnterMap = null!;
        private readonly MissionCompleted MissionCompleted = null!;

        public IStateMachineNode? Execute(State state)
        {
            if (!state.MapGenerators.MoveNext())
                return MissionCompleted;

            state.Map = state.MapGenerators.Current.GenerateMap(state);
            return EnterMap;
        }
    }
}
