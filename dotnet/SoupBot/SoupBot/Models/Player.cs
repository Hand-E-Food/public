using SoupBot.Data;
using SoupBot.Brains;

namespace SoupBot.Models;
/// <summary>
/// The player-controlled robot.
/// </summary>
public class Player : Robot
{
    protected override IBrain[] Brains { get; }

    public override Image Image { get; }

    public override string Name => "Soup Master";

    public Player(IAssetRepository assetRepository, IBrainFactory brainFactory)
    {
        Brains = new[]
        {
            brainFactory.Create<PlayerInputBrain>(this),
        };
        Image = assetRepository.GetImage("player");
    }
}
