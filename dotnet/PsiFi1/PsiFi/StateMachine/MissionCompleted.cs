using PsiFi.Models;
using System;

namespace PsiFi.StateMachine
{
    class MissionCompleted : IStateMachineNode
    {
        public IStateMachineNode? Execute(State state)
        {
            Console.Clear();
            return null;
        }
    }
}
