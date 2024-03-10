using PsiFi.Mapping;
using Rogue;
using Rogue.ConsoleForms;

namespace PsiFi.ConsoleForms
{
    public class MapView : MapView<Cell>
    {
        /// <inheritdoc/>
        protected override void PaintCell(Cell cell)
        {
            if (!cell.IsExplored)
            {
                RogueConsole.Write(Color.Black, Color.Gray, ' ');
            }
            else if (!cell.IsVisible)
            {
                RogueConsole.Write(Color.Black, DarkColor(cell.ForeColor), cell.Character);
            }
            else
            {
                IPhysical topMostObject;
                if (cell.Mob != null)
                    topMostObject = cell.Mob;
                else
                    topMostObject = cell;

                RogueConsole.Write(cell.BackColor, topMostObject.ForeColor, topMostObject.Character);
            }
        }

        /// <summary>
        /// Converts the color to one of the dark shades.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A dark color.</returns>
        private static Color DarkColor(Color color) =>
            new Color(unchecked((byte)(color.R / 2)), unchecked((byte)(color.G / 2)), unchecked((byte)(color.B / 2)));
    }
}
