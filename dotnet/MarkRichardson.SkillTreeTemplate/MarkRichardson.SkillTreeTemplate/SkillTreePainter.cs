using MarkRichardson.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace MarkRichardson.SkillTreeTemplate
{

    /// <summary>
    /// The base class for painting skill trees.
    /// </summary>
    public abstract class SkillTreePainter : IDisposable
    {

        /// <summary>
        /// The margin to add to the Skill Tree.  Should encompass the width of the circles' lines.
        /// </summary>
        protected const float Margin = 0.003f;

        /// <summary>
        /// The radius of each circle.
        /// </summary>
        protected static readonly float[] Radius = { 0f, 0.040f, 0.045f, 0.050f, 0.055f };

        protected readonly Pen _areaPen = new Pen(Color.Black, 0.001f);
        protected readonly Pen _arrowPen = new Pen(Color.Black, 0.01f) { EndCap = LineCap.ArrowAnchor };
        protected readonly Brush _circleBrush = new SolidBrush(Color.White);
        protected readonly Pen _circlePen = new Pen(Color.Black, 0.002f);
        protected readonly Font _labelFont = new Font(FontFamily.GenericSansSerif, 0.024f, FontStyle.Bold);
        protected readonly Brush _textBrush = new SolidBrush(Color.Black);
        protected readonly Font _textFont = new Font(FontFamily.GenericSansSerif, 0.016f);
        protected readonly StringFormat _textFormat = new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap) { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        /// <summary>
        /// The areas defined for each field.
        /// </summary>
        protected ShapeCollection<Circle> Areas { get; private set; }

        /// <summary>
        /// A list of all arrows to draw.
        /// </summary>
        protected ShapeCollection<Line> Arrows { get; private set; }

        /// <summary>
        /// The text to display in each circle.
        /// </summary>
        protected Dictionary<string, string> CircleText { get; set; }

        /// <summary>
        /// A list of all circles to draw.
        /// </summary>
        protected ShapeCollection<Circle> Circles { get; private set; }

        /// <summary>
        /// A list of all labels to draw.
        /// </summary>
        protected ShapeCollection<Label> Labels { get; private set; }

        /// <summary>
        /// The bounds encompasing all circles.
        /// </summary>
        private RectangleF _logicalBounds;

        /// <summary>
        /// Initialises a new intance of the <see cref="SkillTreePainter"/> class.
        /// </summary>
        public SkillTreePainter()
        {
            Areas = new ShapeCollection<Circle>();
            Arrows = new ShapeCollection<Line>();
            Circles = new ShapeCollection<Circle>();
            Labels = new ShapeCollection<Label>();
        }

        /// <summary>
        /// Calculates the logical bounds of the canvas that encapsulates all shapes.
        /// </summary>
        protected void CalculateLogicalBounds()
        {
            var allBounds = Enumerable.Empty<Shape>()
                .Concat(Areas.Values)
                .Concat(Arrows.Values)
                .Concat(Circles.Values)
                .Concat(Labels.Values)
                .Select(x => x.Bounds)
                .ToArray();

            var topLeft = new PointF(
                allBounds.Min(c => c.Left) - Margin,
                allBounds.Min(c => c.Top) - Margin);
            var size = new SizeF(
                allBounds.Max(c => c.Right) - topLeft.X + Margin * 2f,
                allBounds.Max(c => c.Bottom) - topLeft.Y + Margin * 2f);
            _logicalBounds = new RectangleF(topLeft, size);
        }

        /// <summary>
        /// Creates a new arrow with default properties and adds it to the arrows collection.
        /// </summary>
        /// <param name="start">The start of the arrow.</param>
        /// <param name="end">The endof the arrow.</param>
        /// <returns>The initialised arrow.</returns>
        protected Line CreateArrow(Circle start, Circle end)
        {
            return Arrows.Add(Arrows.Count.ToString(), new Line
            {
                Start = start,
                End = end,
                Pen = _arrowPen,
            });
        }

        /// <summary>
        /// Creates a new circle with default properties and adds it to the circles collection.
        /// </summary>
        /// <param name="field">The circle's field.</param>
        /// <param name="tier">The circle's tier.</param>
        /// <param name="skill">The circle'sskill sequence number.</param>
        /// <param name="centre">The centre of the circle.</param>
        /// <returns>The initialised circle.</returns>
        protected Circle CreateCircle(string field, int tier, int skill, PointF centre)
        {
            var id = Id(field, tier, skill);
            string text;
            if (!CircleText.TryGetValue(id, out text))
                text = "";

            Labels.Add(id, new Label
            {
                Text = text,
                Centre = centre,
                Font = _textFont,
                Brush = _textBrush,
            });
            return Circles.Add(id, new Circle
            {
                Centre = centre,
                Radius = Radius[tier],
                Brush = _circleBrush,
                Pen = _circlePen,
            });
        }

        /// <summary>
        /// Paints the skill tree to the graphics canvas.
        /// </summary>
        /// <param name="g">The graphics canvas to paint to.</param>
        public void Paint(Graphics g, RectangleF canvasBounds)
        {
            var graphicsState = g.Save();
            var xScale = canvasBounds.Width / _logicalBounds.Width;
            var yScale = canvasBounds.Height / _logicalBounds.Height;
            var scale = Math.Min(xScale, yScale);
            try
            {
                g.TranslateTransform(canvasBounds.X, canvasBounds.Y);
                g.ScaleTransform(scale, scale);
                g.TranslateTransform(-_logicalBounds.Left, -_logicalBounds.Top);
                if (xScale > yScale)
                    g.TranslateTransform((canvasBounds.Width / scale - _logicalBounds.Width) / 2f, 0f);
                else
                    g.TranslateTransform(0f, (canvasBounds.Height / scale - _logicalBounds.Height) / 2f);

                PaintSkillTree(g, scale);
            }
            finally
            {
                g.Restore(graphicsState);
            }
        }

        /// <summary>
        /// Paints the skill tree to the prepared graphics object.
        /// </summary>
        /// <param name="g">The graphics object to paint to.</param>
        /// <param name="scale">The scale ratio between the logical bounds and the canvas bounds.</param>
        private void PaintSkillTree(Graphics g, float scale)
        {
            Areas.FillAndDraw(g);
            Arrows.Draw(g);
            Circles.FillAndDraw(g);
            Labels.Fill(g);
        }

        /// <summary>
        /// Sorts and joins a set of indexed strings.
        /// </summary>
        /// <param name="strings">The source strings.</param>
        /// <param name="indices">The indicies of the strings to return.</param>
        /// <returns>The strings referenced by the indicies in order and joined.</returns>
        protected static string Combine(string[] strings, params int[] indices)
        {
            return string.Join("", indices
                .Select(i => strings.ElementAt(i))
                .OrderBy(f => f)
            );
        }

        /// <summary>
        /// Returns a circle ID.
        /// </summary>
        /// <returns>An ID.</returns>
        protected static string Id(string field, int tier)
        {
            return string.Format("{0}{1}", field, tier);
        }

        /// <summary>
        /// Returns a circle ID.
        /// </summary>
        /// <returns>An ID.</returns>
        protected static string Id(string field, int tier, int number)
        {
            return string.Format("{0}{1}.{2}", field, tier, number);
        }

        /// <summary>
        /// Ensures an angle between 0 and 180 degrees is returned.
        /// </summary>
        /// <param name="deg">The angle to normalize.</param>
        /// <returns>The angle adjusted to be between 0 and 180 degrees.</returns>
        protected static float UprightDeg(float deg)
        {
            return deg - (float)Math.Round(deg / 180f) * 180f;
        }

        /// <summary>
        /// Releases all resources used by this class.
        /// </summary>
        public void Dispose()
        {
            if (_areaPen != null) _areaPen.Dispose();
            if (_arrowPen != null) _arrowPen.Dispose();
            if (_circleBrush != null) _circleBrush.Dispose();
            if (_circlePen != null) _circlePen.Dispose();
            if (_labelFont != null) _labelFont.Dispose();
            if (_textBrush != null) _textBrush.Dispose();
            if (_textFont != null) _textFont.Dispose();
            if (_textFormat != null) _textFormat.Dispose();
        }
    }
}
