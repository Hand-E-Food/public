using Bots.Models;

namespace Bots.Engine;
public class GameMaster
{
    private IEnumerable<Bot> Bots
    {
        get
        {
            foreach (var cell in Map.Cells)
                if (cell.Bot is not null)
                    yield return cell.Bot;
        }
    }
    required public Map Map { get; init; }

    public void NextTurn()
    {
        foreach (var bot in Bots)
        {
            while(!bot.Act());
        }
    }
}
