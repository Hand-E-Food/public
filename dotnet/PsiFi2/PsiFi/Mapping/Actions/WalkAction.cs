using PsiFi.ConsoleForms;

namespace PsiFi.Mapping.Actions
{
    /// <summary>
    /// The action of a mob walking to a cell.
    /// </summary>
    public class WalkAction : IAction
    {
        private readonly Cell cell;
        private readonly Mob mob;

        /// <param name="mob">The mob that is walking.</param>
        /// <param name="cell">The cell the mob is walking to.</param>
        public WalkAction(Mob mob, Cell cell)
        {
            this.cell = cell;
            this.mob = mob;
        }

        /// <inheritdoc/>
        public void Perform(MapScreen mapScreen)
        {
            if (cell.Mob != null || !cell.IsWalkable)
                
            mob.Cell = cell;
            mapScreen.InvalidateMapView();
        }
    }
}
