using System.Drawing;

namespace Bots.Models;
public class Bot
{
    required public char Character { get; init; }
    required public Color ForeColor { get; init; }
    required public Map Map { get; init; }
    required public Pose Pose { 
        get => pose; 
        set
        {
            if (pose != Pose.Empty) Map[pose].Bot = null;
            pose = value;
            if (pose != Pose.Empty) Map[pose].Bot = this;
        }
    }
    private Pose pose;

    public bool Act()
    {
        return false;
    }
}
