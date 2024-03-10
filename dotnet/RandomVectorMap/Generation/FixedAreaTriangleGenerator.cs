using System;
using System.Drawing;
using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation
{
    public class FixedAreaTriangleGenerator : TriangleGenerator
    {

        /// <summary>
        /// Generates the third vertex of a triangle.
        /// </summary>
        /// <param name="pointA">The triangle's first vertex.</param>
        /// <param name="pointB">The triangle's second vertex.</param>
        /// <param name="pointBias">The point away from which the triangle's thrid point should be created.</param>
        /// <returns>The triangle's third vertex.</returns>
        public override Point GenerateTriangle(Point pointA, Point pointB, Point pointBias)
        {
            // A and B are two points of triangle.
            // C and D form a rectangle with A and B.
            // E is the third point of the triangle and lies on the line CD.
            // F is the midpoint of line AB.

            const double EquilateralTriangleRatio = 0.86602540378443864676372317075294; // Math.Sqrt(3/4)
            double area = MaximumRoadLength * MaximumRoadLength * EquilateralTriangleRatio / 2;

            // Measure the line AB.
            Line lineAB = new Line(pointA, pointB);

            if (MaximumRoadLength * 2 < lineAB.Length)
            {
                // If the line is too long to create a triangle, return a flat triangle with the third point at line AB midpoint.
                return GenerateIsoscelesTriangle(pointA, pointB, pointBias, lineAB.Length / 2);
            }

            // Calculate the two possible locations of C.
            double lengthAC = area / lineAB.Length;
            Vector vectorAC = lineAB.Vector.Rotate90() * (lengthAC / lineAB.Length);
            Point pointC1 = pointA + vectorAC;
            Point pointC2 = pointA - vectorAC;

            // Select C furthest from the bias.
            Point pointC;
            if ((new Vector(pointC1, pointBias)).Length > (new Vector(pointC2, pointBias)).Length)
                pointC = pointC1;
            else
                pointC = pointC2;
            Line lineAC = new Line(pointA, pointC);

            // Calculate the location of D and measure lines AD and CD.
            Point pointD = pointB + lineAC.Vector;
            Line lineCD = new Line(pointC, pointD);
            Line lineAD = new Line(pointA, pointD);

            // Detect if the line AE or BE could potentially be too short.
            double excessMin = 0;
            if (lineAC.Length < MinimumRoadLength)
            {
                excessMin = Math.Sqrt(MinimumRoadLength * MinimumRoadLength - lineAC.Length * lineAC.Length);
                if (excessMin > lineAB.Length / 2)
                {
                    // If the triangle is too flat, make a taller isosceles triangle.
                    return GenerateIsoscelesTriangle(pointA, pointB, pointBias, MinimumRoadLength);
                }
            }

            // Detect if the lines AE or BE will be too long.
            double lengthAF = lineAB.Length / 2;
            if (Math.Sqrt(lengthAC * lengthAC + lengthAF * lengthAF) > MaximumRoadLength)
            {
                // If the triangle is too tall, make a shorter isosceles triangle.
                return GenerateIsoscelesTriangle(pointA, pointB, pointBias, MaximumRoadLength);
            }
            double excessMax = 0;
            if (lineAD.Length > MaximumRoadLength)
            {
                // If the maximum line length cannot reach the corners of the rectangle, use a trapezium instead.
                excessMax = lineCD.Length - Math.Sqrt(MaximumRoadLength * MaximumRoadLength - lengthAC * lengthAC);
                if (excessMax > lineAB.Length / 2)
                {
                    // If the triangle is too wide, make a narrower isosceles triangle.
                    return GenerateIsoscelesTriangle(pointA, pointB, pointBias, MaximumRoadLength);
                }
            }

            double excessMost = Math.Max(excessMin, excessMax);
            if (excessMost > 0)
            {
                // If the line CD must be altered, do so.
                Vector vectorExcess = lineCD.Vector * (excessMost / lineCD.Length);
                pointC = pointC + vectorExcess;
                pointD = pointD - vectorExcess;
                lineCD = new Line(pointC, pointD);
                lineAD = new Line(pointA, pointD);
            }

            // Select a random point on the line between C and D.
            Point pointE = pointC + lineCD.Vector * Random.NextDouble();

            // Return the triangle's third point.
            return pointE;
        }

        /// <summary>
        /// Generates teh third point of an isosceles triangle given two points and a side length.
        /// </summary>
        /// <param name="pointA">The triangle's first vertex.</param>
        /// <param name="pointB">The triangle's second vertex.</param>
        /// <param name="pointBias">The point away from which the triangle's thrid point should be created.</param>
        /// <param name="lengthAE">The length of the two equal-length sides.</param>
        /// <returns>The triangle's third point.</returns>
        private Point GenerateIsoscelesTriangle(Point pointA, Point pointB, Point pointBias, double lengthAE)
        {
            // Measure the line AB.
            Line lineAB = new Line(pointA, pointB);

            // Halve line AB
            Point pointF = pointA + lineAB.Vector / 2;
            Line lineAF = new Line(pointA, pointF);

            // Calculate the distance between line AB and E.
            double lengthFE = Math.Sqrt(lengthAE * lengthAE - lineAF.Length * lineAF.Length);

            // Calculate the two possible locations of E.
            Vector vectorFE = lineAB.Vector.Rotate90() * (lengthFE / lineAB.Length);
            Point pointE1 = pointF + vectorFE;
            Point pointE2 = pointF - vectorFE;

            // Return the E that is furthest from the bias.
            if ((new Vector(pointE1, pointBias)).Length > (new Vector(pointE2, pointBias)).Length)
                return pointE1;
            else
                return pointE2;
        }
    }
}
