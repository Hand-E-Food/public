using SoupBot.Mapping;
using SoupBot.Models;

namespace SoupBot.Brains;
public class PlayerInputBrain : Brain
{
    private readonly Map map;
    private readonly IPlayerInput playerInput;

    public PlayerInputBrain(Map map, IPlayerInput playerInput)
    {
        this.map = map;
        this.playerInput = playerInput;
    }

    public override bool Act()
    {
        var dx = playerInput.Horizontal;
        if (dx != 0)
        {
            Size move = new(dx, 0);
            Point destination = Robot.Location + move;
            if (!map[destination].IsSolid)
            {
                Robot.Location = destination;
                Robot.BeBusyFor(Robot.Duration.Walk);
                return true;
            }
        }
        return false;
    }
}
