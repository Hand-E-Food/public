namespace SoupBot.Models;

public struct MapCell
{
    required public Image Image { get; set; }
    required public bool IsSolid { get; set; }
}
