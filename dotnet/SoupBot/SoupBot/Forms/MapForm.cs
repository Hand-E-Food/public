using SoupBot.Brains;
using SoupBot.Engine;
using SoupBot.Mapping;
using System.ComponentModel;

namespace SoupBot.Forms;

public partial class MapForm : Form
{
    private readonly IGameEngine engine = null!;
    private readonly Dictionary<Keys, PropertyWrapper<bool>> keyMappings = null!;
    private readonly EventWaitHandle closingEventHandle = new ManualResetEvent(false);

    /// <summary>
    /// The displayed map.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Map? Map

    {
        get => mapControl.Map;
        init
        {
            var map = value;
            mapControl.Map = map;
            Invalidate();
            if (map == null) return;
            if (Screen.PrimaryScreen is not null)
            {
                var screenBounds = Screen.PrimaryScreen.Bounds;
                var multiplier = Math.Max(1, Math.Min(
                    screenBounds.Width / (map.Size.Width * Constants.TilePixels),
                    screenBounds.Height / (map.Size.Height * Constants.TilePixels)
                ));
                ClientSize = map.Size * multiplier * Constants.TilePixels;
            }
        }
    }

    /// <summary>
    /// The inputs provided by the player.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public PlayerInput PlayerInput
    {
        init
        {
            if (value != null)
            {
                keyMappings = new() {
                    { Keys.Down, PropertyWrapper.For(value, x => x.Down) },
                    { Keys.Left, PropertyWrapper.For(value, x => x.Left) },
                    { Keys.Right, PropertyWrapper.For(value, x => x.Right) },
                    { Keys.Up, PropertyWrapper.For(value, x => x.Up) },
                };
            }
            else
            {
                keyMappings = new();
            }
        }
    }

    private MapForm()
    {
        InitializeComponent();
    }

    [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
    public MapForm(IGameEngine engine, Map map, PlayerInput playerInput) : this()
    {
        this.engine = engine;
        Map = map;
        PlayerInput = playerInput;
    }

    private void MapForm_Shown(object sender, EventArgs e)
    {
        scheduler.RunWorkerAsync();
    }

    private void MapForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (keyMappings.TryGetValue(e.KeyCode, out var mapping))
            mapping.Value = true;
    }

    private void MapForm_KeyUp(object sender, KeyEventArgs e)
    {
        if (keyMappings.TryGetValue(e.KeyCode, out var mapping))
            mapping.Value = false;
    }

    private void scheduler_DoWork(object sender, DoWorkEventArgs e)
    {
        TimeSpan delay = TimeSpan.Zero;
        while (!closingEventHandle.WaitOne(delay))
        {
            DateTime next = engine.Act();
            mapControl.Invalidate();
            delay = next - DateTime.UtcNow;
        }
        closingEventHandle.Dispose();
    }

    private void MapForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        closingEventHandle.Set();
    }
}
