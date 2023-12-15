using SoupBot.Models;

namespace SoupBot.Generation;

public interface IMapGenerator
{
    Map GenerateMap();
}

public class MapGenerator : IMapGenerator
{
    private readonly Map map;
    private readonly IMapCellGenerator mapCellGenerator;
    private readonly Random random = new();

    public MapGenerator(IMapCellGenerator mapCellGenerator)
    {
        this.mapCellGenerator = mapCellGenerator;
        Size size = new(30, 30);
        map = new(size);
    }

    public Map GenerateMap()
    {
        CreateSurface();
        return map;
    }

    private void CreateSurface()
    {
        int xMid = map.Size.Width / 2;
        int ySurface = 10;
        CreateSurface(xMid, ySurface);
        CreateSurface(Enumerable.Range(0, xMid).Reverse(), ySurface);
        CreateSurface(Enumerable.Range(xMid, map.Size.Width - xMid), ySurface);
    }

    private void CreateSurface(IEnumerable<int> xs, int ySurface)
    {
        double y = ySurface;
        double dY = 0;
        foreach (int x in xs)
        {
            dY += random.NextDouble() - 0.5 - dY / 4;
            y += dY;
            int yInt = Math.Clamp((int)Math.Round(y), 0, map.Size.Height);
            CreateSurface(x, yInt);
        }
    }

    private void CreateSurface(int x, int ySurface)
    {
        int y = 0;
        for (; y < ySurface; y++)
            map.Cells[x, y] = mapCellGenerator.Sky();
        for (; y < map.Size.Height; y++)
            map.Cells[x, y] = mapCellGenerator.Dirt();
    }
}
