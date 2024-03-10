using PsiFi.Geometry;
using PsiFi.Models;
using PsiFi.Models.Mobs;
using PsiFi.Rules;
using System.Collections.Generic;
using System.Linq;
using static PsiFi.Geometry.Math;

namespace PsiFi.Brains
{
    class AnimalBrain : IBrain
    {
        protected Mob Self { get; }

        public AnimalBrain(Mob self)
        {
            Self = self;
        }

        public void Act(State state)
        {
            var visibleCells = GetVisibleCells(state);

            var enemyCell = visibleCells.FirstOrDefault(cell => IsEnemy(cell.Occupant));
            if (enemyCell != null)
            {
                var adjacentCells = Vector.AllUnits
                    .Select(vector => state.Map[Self.Cell!.Location + vector])
                    .OrderBy(cell => (cell.Location - enemyCell.Location).Distance)
                    .ToList();

                if (adjacentCells[0] == enemyCell)
                {
                    Self.ActTime += Self.MeleeDuration;
                    Actions.Attack(Attack.From(Self).MeleeTo(enemyCell).IfHits(Self.MeleeEffects));
                    return;
                }

                var targetCell = adjacentCells.FirstOrDefault(cell => cell.Occupant == null);

                if (targetCell != null && Actions.Walk(Self, targetCell))
                {
                    Self.ActTime += Self.WalkDuration;
                    return;
                }
            }
            else
            {
                var directions = state.Random.Shuffle(Vector.AllUnits);
                foreach (var direction in directions)
                {
                    if (Actions.Walk(state.Map, Self, direction))
                    {
                        Self.ActTime += Self.WalkDuration;
                        return;
                    }
                }
            }
            Actions.Wait(state.Map, Self);
        }

        private List<Cell> GetVisibleCells(State state)
        {
            var visions = state.Player.GetVisions().ToList();

            return state.Map.AllCells
                .Where(cell => CalculateVisibility(cell, visions, state.Map) > CellVisibility.NotVisible)
                .OrderBy(cell => (Self.Cell!.Location - cell.Location).Distance)
                .ToList();
        }

        protected virtual bool IsEnemy(Occupant? occupant) => occupant is Player;
    }
}
