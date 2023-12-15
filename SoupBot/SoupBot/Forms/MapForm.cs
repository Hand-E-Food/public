using SoupBot.Models;

namespace SoupBot.Forms;

public partial class MapForm : Form
{
    public Map? Map
    {
        get => mapControl.Map;
        set
        {
            var map = value;
            mapControl.Map = map;
            Invalidate();
            if (map is null) return;
            if (Screen.PrimaryScreen is null) return;
            var screenBounds = Screen.PrimaryScreen.Bounds;
            var multiplier = Math.Max(1, Math.Min(
                screenBounds.Width / (map.Size.Width * Constants.TilePixels),
                screenBounds.Height / (map.Size.Height * Constants.TilePixels)
            ));
            ClientSize = map.Size * multiplier * Constants.TilePixels;
        }
    }
    
    public MapForm()
    {
        InitializeComponent();
    }

    public MapForm(Map map)
        : this()
    {
        Map = map;
    }
}
