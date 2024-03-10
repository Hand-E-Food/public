namespace SoupBot.Data;

public interface IAssetRepository
{
    Image GetImage(string name);
}

public class AssetRepository : IAssetRepository
{
    private const string AssetsFolder = "Assets";

    private readonly Dictionary<string, Image> imageCache = new();

    public Image GetImage(string name)
    {
        if (!imageCache.TryGetValue(name, out Image? image))
        {
            var path = Path.Combine(AssetsFolder, name + ".png");
            image = Image.FromFile(path);
            imageCache.Add(name, image);
        }
        return image;
    }
}
