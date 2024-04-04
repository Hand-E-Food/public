using Bots.Engine;
using Bots.Models;

namespace Bots.Forms;
public partial class MapForm : Form
{
    public GameMaster? GameMaster { get; init; }

    public Map? Map { get; init; }

    public MapForm() => InitializeComponent();

    public void UpdateMap()
    {
        mapTextBox.Clear();
        if (Map is null) return;

        using var _ = mapTextBox.SuspendedLayout();
        for (int y = 0; y < Map.Size.Height; y++)
        {
            if (y != 0) mapTextBox.AppendText(Environment.NewLine);
            for (int x = 0; x < Map.Size.Width; x++)
            {
                var cell = Map.Cells[y, x];
                Color foreColor = Color.Black;
                Color backColor = Color.LawnGreen;
                char character = ' ';
                if (cell.Bot is not null)
                {
                    foreColor = cell.Bot.ForeColor;
                    character = cell.Bot.Character;
                }
                mapTextBox.SelectionBackColor = backColor;
                mapTextBox.SelectionColor = foreColor;
                mapTextBox.AppendText(character.ToString());
            }
        }
    }

    private void mapTextBox_Click(object sender, EventArgs e)
    {
        if (GameMaster is null) return;
        GameMaster.NextTurn();
        UpdateMap();
    }
}
