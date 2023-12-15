using SoupBot.Data;
using SoupBot.Models;

namespace SoupBot.Generation;

public interface IMapCellGenerator
{
    MapCell Dirt();
    MapCell Sky();
}

public class MapCellGenerator : IMapCellGenerator
{
    private readonly IAssetRepository assetRepository;

    public MapCellGenerator(IAssetRepository assetRepository)
    {
        this.assetRepository = assetRepository;
    }

    public MapCell Dirt() => new()
    {
        Image = assetRepository.GetImage("dirt"),
        IsSolid = true,
    };

    public MapCell Sky() => new()
    {
        Image = assetRepository.GetImage("sky"),
        IsSolid = false,
    };
}
