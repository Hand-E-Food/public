namespace SoupBot.Models;

public class Map
{
    public MapCell[,] Cells { get; }
 
    public Robot? Player { get; set; }

    public Size Size { get; }

    public Map(Size size)
    {
        Size = size;
        Cells = new MapCell[size.Width, size.Height];
    }
}
