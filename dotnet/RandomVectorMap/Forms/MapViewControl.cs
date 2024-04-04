using RandomVectorMap.Mapping;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;

namespace RandomVectorMap.Forms;

/// <summary>
/// Displays a map.
/// </summary>
public partial class MapViewControl : UserControl
{
    /// <summary>
    /// Initialises a new instnace of the MapViewControl class.
    /// </summary>
    public MapViewControl()
    {
        InitializeComponent();

        BiomeBrushes = new()
        {
            { Biome.Undefined, new HatchBrush(HatchStyle.SmallCheckerBoard, Color.White, Color.LightGray) },
            { Biome.Ocean, new HatchBrush(HatchStyle.Wave, Color.SkyBlue, Color.DeepSkyBlue) },
            { Biome.Lake, new HatchBrush(HatchStyle.Wave, Color.SkyBlue, Color.DeepSkyBlue) },
            { Biome.Desert, new HatchBrush(HatchStyle.SmallConfetti, Color.LightYellow, Color.Yellow) },
            { Biome.Pasture, new HatchBrush(HatchStyle.SmallConfetti, Color.PaleVioletRed, Color.LawnGreen) },
            { Biome.Swamp, new HatchBrush(HatchStyle.Wave, Color.SeaGreen, Color.DarkSeaGreen) },
            { Biome.Forest, new HatchBrush(HatchStyle.Divot, Color.BurlyWood, Color.ForestGreen) },
            { Biome.Mountain, new HatchBrush(HatchStyle.ZigZag, Color.LightGray, Color.Gray) },
            { Biome.Snow, new HatchBrush(HatchStyle.LargeConfetti, Color.Snow, Color.White) }
        };

        SettlementBrushes = new()
        {
            { SettlementSize.Service, new HatchBrush(HatchStyle.LargeConfetti, Color.FromArgb(255, Color.SteelBlue), Color.Transparent) },
            { SettlementSize.Homestead, new HatchBrush(HatchStyle.Shingle, Color.FromArgb(128, Color.Brown), Color.FromArgb(64, Color.Brown)) },
            { SettlementSize.Town, new HatchBrush(HatchStyle.LargeGrid, Color.FromArgb(255, Color.DarkGray), Color.FromArgb(64, Color.Gray)) },
            { SettlementSize.City, new HatchBrush(HatchStyle.SmallGrid, Color.FromArgb(255, Color.Black), Color.FromArgb(64, Color.Gray)) }
        };

        RoadQualityPens = new()
        {
            { RoadQuality.Undefined, new Pen[] { new(Color.Red, 1) { DashStyle = DashStyle.Dot } } },
            { RoadQuality.None, new Pen[] { new(Color.Transparent, 0) } },
            { RoadQuality.River, new Pen[] { new(Color.DeepSkyBlue, 3) } },
            { RoadQuality.Wild, new Pen[] { new(Color.Transparent, 0) } },
            { RoadQuality.Dirt, new Pen[] { new(Color.SaddleBrown, 2) } },
            { RoadQuality.Paved, new Pen[] { new(Color.Black, 2) } },
            { RoadQuality.Highway, new Pen[] { new(Color.Black, 4), new(Color.Yellow, 0.5f) { DashStyle = DashStyle.Dash } } }
        };

        SettlementSizeSprites = new()
        {
            { SettlementSize.Undefined, new(0, Brushes.Transparent) },
            { SettlementSize.None, new(0, Brushes.Transparent) },
            { SettlementSize.Service, new(6, Brushes.Gray) },
            { SettlementSize.Homestead, new(6, Brushes.Brown) },
            { SettlementSize.Town, new(8, Brushes.Black) },
            { SettlementSize.City, new(12, Brushes.DarkRed) }
        };

        Map = null;
        ShowDebug = true;
        Zoom = 1.0;
    }

    /// <summary>
    /// The brush for each biome.
    /// </summary>
    private readonly Dictionary<Biome, Brush> BiomeBrushes;

    /// <summary>
    /// Gets or sets the geographical point displayed in the centre of the map.
    /// </summary>
    /// <value>The geographical point displayed in the centre of the map.</value>
    public Point CenterPoint
    {
        get => centerPoint;
        set
        {
            if (centerPoint == value) return;
            centerPoint = value;
            OnCenterPointChanged(EventArgs.Empty);
        }
    }
    private Point centerPoint;
    /// <summary>
    /// Raises the CenterPointChanged event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnCenterPointChanged(EventArgs e)
    {
        Invalidate();
        CenterPointChanged?.Invoke(this, e);
    }
    /// <summary>
    /// Raised when the CenterPoint property is changed.
    /// </summary>
    public event EventHandler? CenterPointChanged;

    /// <summary>
    /// Gets or sets the Map to display.
    /// </summary>
    /// <value>The Map to display.</value>
    [DefaultValue(null)]
    public Map? Map
    {
        get => map;
        set
        {
            if (map == value) return;
            map = value;
            OnMapChanged(EventArgs.Empty);
        }
    }
    private Map? map;
    /// <summary>
    /// Raises the MapChanged event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnMapChanged(EventArgs e)
    {
        Invalidate();
        MapChanged?.Invoke(this, e);
    }
    /// <summary>
    /// Raised when the Map property is changed.
    /// </summary>
    public event EventHandler? MapChanged;

    /// <summary>
    /// The pen for each road quality.
    /// </summary>
    private readonly Dictionary<RoadQuality, Pen[]> RoadQualityPens;

    /// <summary>
    /// The brush for zones surrounding each settlement size.
    /// </summary>
    private readonly Dictionary<SettlementSize, Brush> SettlementBrushes;

    /// <summary>
    /// The size and brush for each settlement size.
    /// </summary>
    private readonly Dictionary<SettlementSize, Sprite> SettlementSizeSprites;

    /// <summary>
    /// Gets or sets a value indicating whether or not display debug information.
    /// </summary>
    /// <value>True to show debug information.  Otherwise, false.</value>
    [DefaultValue(true)]
    public bool ShowDebug
    {
        get => showDebug;
        set
        {
            if (showDebug == value) return;
            showDebug = value;
            OnShowDebugChanged(EventArgs.Empty);
        }
    }
    private bool showDebug;
    /// <summary>
    /// Raises the ShowDebugChanged event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnShowDebugChanged(EventArgs e)
    {
        Invalidate();
        ShowDebugChanged?.Invoke(this, e);
    }
    /// <summary>
    /// Raised when the ShowDebug property is changed.
    /// </summary>
    public event EventHandler? ShowDebugChanged;

    /// <summary>
    /// Gets or sets the zoom level.
    /// </summary>
    /// <value>The zoom level.</value>
    [DefaultValue(1.0)]
    public double Zoom
    {
        get { return zoom; }
        set
        {
            if (zoom == value) return;
            zoom = value;
            OnZoomChanged(EventArgs.Empty);
        }
    }
    private double zoom;
    /// <summary>
    /// Raises the ZoomChanged event.
    /// </summary>
    /// <param name="e">The event arguments.</param>
    protected virtual void OnZoomChanged(EventArgs e)
    {
        Invalidate();
        ZoomChanged?.Invoke(this, e);
    }
    /// <summary>
    /// Raised when the Zoom property is changed.
    /// </summary>
    public event EventHandler? ZoomChanged;

    /// <summary>
    /// Paints the map.
    /// </summary>
    private void MapViewControl_Paint(object sender, PaintEventArgs e)
    {
        try
        {
            if (Map is null) return;

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            PaintOutside(e, Map.Outside);
            PaintZones(e, Map.Zones);
            PaintRoads(e, Map.Roads);
            PaintJunctions(e, Map.Junctions);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    /// <summary>
    /// Measures the rectangluar bounds of a set of points.
    /// </summary>
    /// <param name="points">The points to measure.</param>
    /// <returns>A rectangle that encapsulates all points.</returns>
    private static Rectangle MeasureBounds(IEnumerable<PointF> points)
    {
        int x = (int)Math.Floor(points.Min(p => p.X));
        int y = (int)Math.Floor(points.Min(p => p.Y));
        int width = (int)Math.Ceiling(points.Max(p => p.X)) - x;
        int height = (int)Math.Ceiling(points.Max(p => p.Y)) - y;
        return new(x, y, width, height);
    }
    /// <summary>
    /// Measures the rectangluar bounds of a RectangleF.
    /// </summary>
    /// <param name="points">The RectangleF to measure.</param>
    /// <returns>A rectangle that encapsulates the RectangleF.</returns>
    private static Rectangle MeasureBounds(RectangleF rectangle)
    {
        int x = (int)Math.Floor(rectangle.Left);
        int y = (int)Math.Floor(rectangle.Top);
        int width = (int)Math.Ceiling(rectangle.Right) - x;
        int height = (int)Math.Ceiling(rectangle.Bottom) - y;
        return new(x, y, width, height);
    }

    /// <summary>
    /// Paints all junctions.
    /// </summary>
    /// <param name="g">The graphics object to paint.</param>'
    /// <param name="zones">The junctions to paint.</param>
    private void PaintJunctions(PaintEventArgs e, IEnumerable<Junction> junctions)
    {
        foreach (var junction in junctions) PaintJunction(e, junction);
    }

    /// <summary>
    /// Paints a junction.
    /// </summary>
    /// <param name="g">The graphics object to paint.</param>
    /// <param name="junction">The junction to paint.</param>
    private void PaintJunction(PaintEventArgs e, Junction junction)
    {
        // Get the junction's sprite.
        var sprite = SettlementSizeSprites[junction.Size];

        // Measure the junctions's size.
        RectangleF bounds = new(TransformPoint(junction), new(sprite.Size, sprite.Size));
        bounds.Offset(-bounds.Width / 2, -bounds.Height / 2);

        // If the sprite does not intersect with the clip area, don't paint.
        if (!e.ClipRectangle.IntersectsWith(MeasureBounds(bounds))) return;

        // Paint the junction.
        e.Graphics.FillEllipse(sprite.Brush, bounds);

        if (ShowDebug)
        {
            // Paint the debug junction.
            bounds.Location = TransformPoint(junction);
            bounds.Size = new SizeF(9, 9);
            bounds.Offset(-bounds.Width / 2, -bounds.Height / 2);
            using SolidBrush brush = new(junction.DebugColor);
            using Pen pen = new(Color.FromArgb(junction.DebugColor.A, Color.White), 1);
            e.Graphics.FillEllipse(brush, bounds);
            e.Graphics.DrawEllipse(pen, bounds);
        }
    }

    /// <summary>
    /// Paints the outside region.
    /// </summary>
    private void PaintOutside(PaintEventArgs e, Zone? zone)
    {
        // Select a brush.
        Brush brush = BiomeBrushes[zone is not null ? zone.Biome : Biome.Undefined];
        // Paint the polygon.
        e.Graphics.FillRectangle(brush, e.ClipRectangle);
    }

    /// <summary>
    /// Paints all roads.
    /// </summary>
    /// <param name="g">The graphics object to paint.</param>'
    /// <param name="roads">The roads to paint.</param>
    private void PaintRoads(PaintEventArgs e, IEnumerable<Road> roads)
    {
        foreach (var road in roads) PaintRoad(e, road);
    }

    /// <summary>
    /// Paints a road.
    /// </summary>
    /// <param name="g">The graphics object to paint.</param>
    /// <param name="zone">The road to paint.</param>
    private void PaintRoad(PaintEventArgs e, Road road)
    {
        // Get the roads's line.
        var points = road.Junctions.Select(j => TransformPoint(j)).ToArray();

        // If the line does not intersect with the clip area, don't paint.
        if (!e.ClipRectangle.IntersectsWith(MeasureBounds(points))) return;

        // Select the road's pens.
        foreach (Pen pen in RoadQualityPens[road.Quality])
        {
            // Paint the line.
            e.Graphics.DrawLine(pen, points[0], points[1]);
        }

        if (ShowDebug)
        {
            // Paint the debug line.
            e.Graphics.DrawLine(new Pen(Color.FromArgb(road.DebugColor.A, Color.White), 5), points[0], points[1]);
            e.Graphics.DrawLine(new Pen(road.DebugColor, 3), points[0], points[1]);
        }
    }

    /// <summary>
    /// Paints all zones.
    /// </summary>
    /// <param name="g">The graphics object to paint.</param>'
    /// <param name="zones">The zones to paint.</param>
    private void PaintZones(PaintEventArgs e, IEnumerable<Zone> zones)
    {
        foreach (var zone in zones) PaintZone(e, zone);
        if (ShowDebug)
        {
            foreach (var zone in zones) PaintZoneName(e, zone);
        }
    }

    /// <summary>
    /// Paints a zone.
    /// </summary>
    /// <param name="g">The graphics object to paint.</param>
    /// <param name="zone">The region to paint.</param>
    private void PaintZone(PaintEventArgs e, Zone zone)
    {
        var junctions = zone.Junctions;
        if (junctions is null) return;

        // Get the zone's polygon.
        var points = junctions.Select(j => TransformPoint(j)).ToArray();

        // If the polygon does not intersect with the clip area, don't paint.
        if (!e.ClipRectangle.IntersectsWith(MeasureBounds(points))) return;

        // Paint the polygon.
        e.Graphics.FillPolygon(BiomeBrushes[zone.Biome], points);
        if (SettlementBrushes.TryGetValue(zone.SettlementSize, out Brush? brush))
            e.Graphics.FillPolygon(brush, points);

        if (ShowDebug)
        {
            // Draw the debug zone.
            e.Graphics.FillPolygon(new SolidBrush(zone.DebugColor), points);
            e.Graphics.DrawPolygon(new Pen(Color.FromArgb(zone.DebugColor.A, Color.White), 2), points);
        }
    }

    /// <summary>
    /// Paints a zone.
    /// </summary>
    /// <param name="e">The graphics object to paint.</param>
    /// <param name="zone">The region to paint.</param>
    private void PaintZoneName(PaintEventArgs e, Zone zone)
    {
        var junctions = zone.Junctions;
        if (junctions is null) return;

        // Get the zone's polygon.
        var points = junctions.Select(j => TransformPoint(j)).ToArray();

        // If the polygon does not intersect with the clip area, don't paint.
        if (!e.ClipRectangle.IntersectsWith(MeasureBounds(points))) return;

        // Find the polygon's centre.
        PointF centrePoint = new(points.Average(p => p.X), points.Average(p => p.Y));
        // Write the zone's name.
        e.Graphics.DrawString(
            zone.Name,
            Font,
            Brushes.Black,
            centrePoint,
            new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
        );
    }

    /// <summary>
    /// Transforms a point to onscreen coordinates. 
    /// </summary>
    /// <param name="point">The point to transform.</param>
    /// <returns>The transformed point.</returns>
    private PointF TransformPoint(Point point)
    {
        return new(
            (float)((point.X - CenterPoint.X) * Zoom) + Width / 2,
            (float)((point.Y - CenterPoint.Y) * Zoom) + Height / 2
        );
    }

    /// <summary>
    /// A graphical spite.
    /// </summary>
    /// <param name="Size">The sprite's size.</param>
    /// <param name="Brush">The sprite's brush.</param>
    private struct Sprite(int Size, Brush Brush)
    {
        /// <summary>
        /// The sprite's size.
        /// </summary>
        public int Size = Size;

        /// <summary>
        /// The sprite's brush.
        /// </summary>
        public Brush Brush = Brush;
    }
}
