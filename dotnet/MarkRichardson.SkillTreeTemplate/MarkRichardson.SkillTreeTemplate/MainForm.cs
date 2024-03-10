using System;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MarkRichardson.SkillTreeTemplate
{

    /// <summary>
    /// The applications main form.
    /// </summary>
    public partial class MainForm : Form
    {

        /// <summary>
        /// The component used to paint the skill tree.
        /// </summary>
        public SkillTreePainter Painter { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Paints this control.
        /// </summary>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            Painter.Paint(e.Graphics, ClientRectangle);
        }

        /// <summary>
        /// Repaints this control.
        /// </summary>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
