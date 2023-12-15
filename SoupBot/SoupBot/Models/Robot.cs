namespace SoupBot.Models;

public class Robot
{
    required public Image Image { get; set; }
    required public Point Location { get; set; }
    required public string Name { get; set; }
}
