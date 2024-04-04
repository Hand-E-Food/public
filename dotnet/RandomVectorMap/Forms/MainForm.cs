using RandomVectorMap.Generation;
using System.Diagnostics.CodeAnalysis;

namespace RandomVectorMap.Forms;

/// <summary>
/// The main form.
/// </summary>
public partial class MainForm : Form
{
    /// <summary>
    /// The ratio of the screen to scroll when scrolling the map.
    /// </summary>
    private const double MoveStep = 0.4;

    /// <summary>
    /// The portion of the MapView control that should not be used.
    /// </summary>
    private const double PaddingRatio = 0.9;

    /// <summary>
    /// The ratio to zoom by when zooming in or out.
    /// </summary>
    private const double ZoomStep = 2.0;

    /// <summary>
    /// Initialises a new instance of the <see cref="MainForm"/> class.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();
        ShowDebugCheckBox.Checked = MapView.ShowDebug;
    }

    /// <summary>
    /// The map generator.
    /// </summary>
    private MapGenerator? Generator { get; set; }

    /// <summary>
    /// Generates a new random seed.
    /// </summary>
    private void RandomButton_Click(object sender, EventArgs e)
    {
        CreateRandomSeed();
        Reset();
    }

    /// <summary>
    /// Creates a new map with a random seed.
    /// </summary>
    private void ResetButton_Click(object sender, EventArgs e)
    {
        Reset();
    }

    /// <summary>
    /// Performs one step of map generation.
    /// </summary>
    private void StepButton_Click(object sender, EventArgs e)
    {
        if (Generator is null) Reset();
        if (Generator.IsFinished) return;
        Generator.Step();
        UpdateStatus();
        MapView.Invalidate();
    }

    /// <summary>
    /// Finishes the current task.
    /// </summary>
    private void TaskButton_Click(object sender, EventArgs e)
    {
        if (Generator is null) Reset();
        if (Generator.IsFinished) return;
        Generator.FinishTask();
        UpdateStatus();
        MapView.Invalidate();
    }

    /// <summary>
    /// Finishes the map generation.
    /// </summary>
    private void FinishButton_Click(object sender, EventArgs e)
    {
        if (Generator is null) Reset();
        while (!Generator.IsFinished)
        {
            Generator.FinishTask();
            UpdateStatus();
            ZoomFit();
            MapView.Refresh();
        }
    }

    /// <summary>
    /// Shows or hides debug information.
    /// </summary>
    private void ShowDebugCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        MapView.ShowDebug = ShowDebugCheckBox.Checked;
    }

    /// <summary>
    /// Scrolls the map upward.
    /// </summary>
    private void MoveUpButton_Click(object sender, EventArgs e)
    {
        MoveView(0.0, -MoveStep);
    }

    /// <summary>
    /// Scrolls the map downward.
    /// </summary>
    private void MoveDownButton_Click(object sender, EventArgs e)
    {
        MoveView(0.0, MoveStep);
    }

    /// <summary>
    /// Scrolls the map to the left.
    /// </summary>
    private void MoveLeftButton_Click(object sender, EventArgs e)
    {
        MoveView(-MoveStep, 0.0);
    }

    /// <summary>
    /// Scrolls the map to the right.
    /// </summary>
    private void MoveRightButton_Click(object sender, EventArgs e)
    {
        MoveView(MoveStep, 0.0);
    }

    /// <summary>
    /// Zooms in to the map.
    /// </summary>
    private void ZoomInButton_Click(object sender, EventArgs e)
    {
        MapView.Zoom *= ZoomStep;
    }

    /// <summary>
    /// Zooms out of the map.
    /// </summary>
    private void ZoomOutButton_Click(object sender, EventArgs e)
    {
        MapView.Zoom /= ZoomStep;
    }

    /// <summary>
    /// Zooms the map to 1:1.
    /// </summary>
    private void ZoomActualButton_Click(object sender, EventArgs e)
    {
        ZoomActual();
    }

    /// <summary>
    /// Zooms the map to fit everything.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ZoomFitButton_Click(object sender, EventArgs e)
    {
        ZoomFit();
    }

    /// <summary>
    /// Repaints the whole control.
    /// </summary>
    private void MapView_SizeChanged(object sender, EventArgs e)
    {
        MapView.Invalidate();
    }

    /// <summary>
    /// Creates a random seed.
    /// </summary>
    /// <returns>The random seed.</returns>
    private int CreateRandomSeed()
    {
        int seed = DateTime.Now.Ticks.GetHashCode();
        ReseedTextBox.Text = seed.ToString();
        ReseedTextBox.Refresh();
        return seed;
    }

    /// <summary>
    /// Scrolls the map.
    /// </summary>
    /// <param name="deltaX">The horizontal offset as a ratio of the window size.</param>
    /// <param name="deltaY">The vertical offset as a ratio of the window size.</param>
    /// <remarks>The window size is defined as the lesser of the windows width and height.</remarks>
    private void MoveView(double deltaX, double deltaY)
    {
        double scroll = Math.Min(MapView.Width, MapView.Height) / MapView.Zoom;
        MapView.CenterPoint += new Size((int)(deltaX * scroll), (int)(deltaY * scroll));
    }

    /// <summary>
    /// Reseeds the random map.
    /// </summary>
    [MemberNotNull(nameof(Generator))]
    private void Reset()
    {
        if (!int.TryParse(SizeTextBox.Text, out int size))
        {
            size = 100;
            SizeTextBox.Text = size.ToString();
        }

        if (!int.TryParse(ReseedTextBox.Text, out int seed))
        {
            seed = CreateRandomSeed();
        }

        Generator = MapGenerator.Default(size, seed);

        MapView.Map = Generator.Map;
        ZoomFit();

        UpdateStatus();
    }

    /// <summary>
    /// Updates the status field.
    /// </summary>
    private void UpdateStatus()
    {
        if (Generator is null) throw new InvalidOperationException(nameof(Generator) + " is not initialised.");
        StatusLabel.Text = Generator.TaskName;
        StatusLabel.Refresh();
    }

    /// <summary>
    /// Zooms the map so one unit is one pixel.
    /// </summary>
    public void ZoomActual()
    {
        MapView.Zoom = 1.0;
    }

    /// <summary>
    /// Sets the map viewer's zoom level.
    /// </summary>
    public void ZoomFit()
    {

        if (MapView.Map is null) return;
        var junctions = MapView.Map.Junctions;
        if (junctions is null || junctions.Count == 0) return;

        double mapWidth = Math.Max(junctions.Max(j => j.Location.X), -junctions.Min(j => j.Location.X)) * 2;
        double mapHeight = Math.Max(junctions.Max(j => j.Location.Y), -junctions.Min(j => j.Location.Y)) * 2;

        if (mapWidth == 0) mapWidth = 1;
        if (mapHeight == 0) mapHeight = 1;

        MapView.CenterPoint = new(0, 0);
        MapView.Zoom = Math.Min(MapView.Width / mapWidth * PaddingRatio, MapView.Height / mapHeight * PaddingRatio);
    }
}
