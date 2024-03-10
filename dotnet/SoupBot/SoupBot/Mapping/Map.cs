using SoupBot.Models;

namespace SoupBot.Mapping;

/// <summary>
/// A map.
/// </summary>
public class Map
{
    /// <summary>
    /// The map cell at the specified location.
    /// </summary>
    /// <param name="x">The cell's x coordinate.</param>
    /// <param name="y">The cell's y coordinate.</param>
    public MapCell this[int x, int y]
    {
        get => Cells[x, y];
        set => Cells[x, y] = value;
    }

    /// <summary>
    /// The map cell at the specified location.
    /// </summary>
    /// <param name="location">The cell's location.</param>
    public MapCell this[Point location]
    {
        get => Cells[location.X, location.Y];
        set => Cells[location.X, location.Y] = value;
    }

    /// <summary>
    /// This map's cells.
    /// </summary>
    public MapCell[,] Cells { get; }

    /// <summary>
    /// The player on this map.
    /// </summary>
    public Player? Player
    {
        get => player;
        set
        {
            if (player != null) Robots.Remove(player);
            player = value;
            if (player != null) Robots.Insert(0, player);
        }
    }
    private Player? player;

    /// <summary>
    /// The robots on this map.
    /// </summary>
    public List<Robot> Robots { get; } = new();

    /// <summary>
    /// This map's size.
    /// </summary>
    public Size Size { get; }

    public Map(Size size)
    {
        Size = size;
        Cells = new MapCell[size.Width, size.Height];
    }
}
