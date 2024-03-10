using SoupBot.Models;

namespace SoupBot.Brains;
public abstract class Brain : IBrain
{
    public Robot Robot { get; set; } = null!;

    public abstract bool Act();
}
