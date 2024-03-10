using PsiFi.Generators.Maps;
using PsiFi.Models;
using PsiFi.Models.Mobs;
using System;
using System.Collections.Generic;

namespace PsiFi.StateMachine
{
    [InitialStateMachineNode]
    class CreateNewGame : IStateMachineNode
    {
        private readonly GenerateMap GenerateMap = null!;

        public IStateMachineNode? Execute(State state)
        {
            var player = CreatePlayer();
            state.MapGenerators = CreateMapGenerators();
            state.Player = player;
            return GenerateMap;
        }

        private IEnumerator<IMapGenerator> CreateMapGenerators()
        {
            yield return new PaintBallArena();
        }

        private Player CreatePlayer()
        {
            var player = new Player
            {
                MaximumHealth = 10,
                Health = 10,
            };
            return player;
        }
    }
}
