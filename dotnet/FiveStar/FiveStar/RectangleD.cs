using System;
using System.Collections.Generic;
using System.Text;

namespace FiveStar
{
    public struct RectangleD
    {
        public double X;
        public double Y;
        public double Width;
        public double Height;

        public RectangleD(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
