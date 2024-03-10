using PsiFi.Geometry;
using PsiFi.Models;
using PsiFi.View;
using System;
using System.Linq;
using static PsiFi.Geometry.Math;

namespace PsiFi.StateMachine
{
    class SelectPlayerAction : IStateMachineNode
    {
        private readonly ExitMap ExitMap = null!;
        private readonly SelectNextActor SelectNextActor = null!;
        
        private readonly KeyboardInput keyboardInput;
        private State state = null!;

        public SelectPlayerAction(KeyboardInput keyboardInput)
        {
            this.keyboardInput = keyboardInput;
        }

        /// <inheritdoc/>
        public IStateMachineNode? Execute(State state)
        {
            this.state = state;
            ResetVisibility();
            return GetInput();
        }

        private void ResetVisibility()
        {
            var visions = state.Player.GetVisions().ToList();

            foreach (var cell in state.Map.AllCells)
            {
                cell.Visibility = CalculateVisibility(cell, visions, state.Map);
                if (cell.Visibility > CellVisibility.NotVisible)
                    cell.Explored = true;
            }
        }

        private IStateMachineNode GetInput()
        {
            while (true)
            {
                var key = keyboardInput.GetKeyPress();
                switch (key.Key)
                {
                    case ConsoleKey.NumPad7:
                    case ConsoleKey.Home:
                        return Interact(Vector.NW);
                    case ConsoleKey.NumPad8:
                    case ConsoleKey.UpArrow:
                        return Interact(Vector.N);
                    case ConsoleKey.NumPad9:
                    case ConsoleKey.PageUp:
                        return Interact(Vector.NE);
                    case ConsoleKey.NumPad6:
                    case ConsoleKey.RightArrow:
                        return Interact(Vector.E);
                    case ConsoleKey.NumPad3:
                    case ConsoleKey.PageDown:
                        return Interact(Vector.SE);
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.DownArrow:
                        return Interact(Vector.S);
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.End:
                        return Interact(Vector.SW);
                    case ConsoleKey.NumPad4:
                    case ConsoleKey.LeftArrow:
                        return Interact(Vector.W);
                    case ConsoleKey.NumPad5:
                        return Wait();
                    case ConsoleKey.Q:
                        return ExitMap;
                }
            }
        }

        private IStateMachineNode Interact(Vector direction)
        {
            var map = state.Map;
            var player = state.Player;

            if (Actions.Walk(map, player, direction))
            {
                player.ActTime += player.WalkDuration;
                return SelectNextActor;
            }
            else
            {
                return Wait();
            }
        }

        private IStateMachineNode Wait()
        {
            Actions.Wait(state.Map, state.Player);
            return SelectNextActor;
        }
    }
}