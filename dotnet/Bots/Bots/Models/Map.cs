using System.Drawing;

namespace Bots.Models;
public partial class Map
{
    public MapCell this[Pose pose] => this[pose.Location];
    public MapCell this[Point point] => this[point.X, point.Y];
    public MapCell this[int x, int y] => Cells[y, x];
    public MapCell[,] Cells { get; }
    public Size Size { get; }

    public Map(Size size)
    {
        Size = size;
        Cells = new MapCell[size.Height, size.Width];
        for (int y = 0; y < size.Height; y++)
            for (int x = 0; x < size.Width; x++)
                Cells[y, x] = new();
    }
}
