using SoupBot.Data;
using SoupBot.Models;

namespace SoupBot.Generation;

public interface IRobotGenerator
{
    Robot GeneratePlayer();
}

public class RobotGenerator
{
    private readonly IAssetRepository assetRepository;

    public RobotGenerator(IAssetRepository assetRepository)
    {
        this.assetRepository = assetRepository;
    }

    public Robot GeneratePlayer(Point location) => new()
    {
        Image = assetRepository.GetImage("player"),
        Location = location,
        Name = "Soup Master",
    };
}
