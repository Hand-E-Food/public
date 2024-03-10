using System;
using System.Drawing;
using System.Windows.Forms;

namespace NodeNetwork.Designer
{
    public class LabelEditor : TextBox
    {

        public Point Center
        {
            get => _center;
            set
            {
                _center = value;
                CalculateLocation();
            }
        }
        private Point _center;

        public LabelEditor()
        {
            AutoSize = true;
            TextAlign = HorizontalAlignment.Center;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            CalculateLocation();
            base.OnSizeChanged(e);
        }

        private void CalculateLocation()
        {
            Location = new Point(Center.X - Width / 2, Center.Y - Height / 2);
        }
    }
}
