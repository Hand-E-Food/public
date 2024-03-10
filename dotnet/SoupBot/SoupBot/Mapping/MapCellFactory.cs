using SoupBot.Data;
using SoupBot.Models;

namespace SoupBot.Mapping;

public interface IMapCellFactory
{
    MapCell Dirt();
    MapCell Sky();
}

public class MapCellFactory : IMapCellFactory
{
    private readonly IAssetRepository assetRepository;

    public MapCellFactory(IAssetRepository assetRepository)
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
