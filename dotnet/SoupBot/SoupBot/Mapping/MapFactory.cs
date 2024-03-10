namespace SoupBot.Mapping;

public interface IMapFactory
{
    Map CreateMap();
}

public class MapFactory : IMapFactory
{
    private readonly Map map;
    private readonly IMapCellFactory mapCellFactory;
    private readonly Random random = new();

    public MapFactory(IMapCellFactory mapCellFactory)
    {
        this.mapCellFactory = mapCellFactory;
        Size size = new(30, 30);
        map = new(size);
    }

    public Map CreateMap()
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
            map[x, y] = mapCellFactory.Sky();
        for (; y < map.Size.Height; y++)
            map[x, y] = mapCellFactory.Dirt();
    }
}
