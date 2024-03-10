using SoupBot.Mapping;
using System.ComponentModel;

namespace SoupBot.Forms;

public partial class MapControl : UserControl
{
    public MapControl()
    {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Map? Map
    {
        get => map;
        set
        {
            map = value;
            Invalidate();
        }
    }
    private Map? map;

    private int tileSize = 1;

    protected override void OnLayout(LayoutEventArgs e)
    {
        base.OnLayout(e);
        if (map is null) return;
        tileSize = Math.Max(1, Math.Min(Width / map.Size.Width, Height / map.Size.Height));
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (map is null) return;
        for (int x = 0; x < map.Size.Width; x++)
        {
            for (int y = 0; y < map.Size.Height; y++)
            {
                Image image = map.Cells[x, y].Image;
                Rectangle rect = new(x * tileSize, y * tileSize, tileSize, tileSize);
                e.Graphics.DrawImage(image, rect);
            }
        }
        foreach (var robot in map.Robots)
        {
            Image image = robot.Image;
            Rectangle rect = new(robot.Location.X * tileSize, robot.Location.Y * tileSize, tileSize, tileSize);
            e.Graphics.DrawImage(image, rect);
        }
        base.OnPaint(e);
    }
}
