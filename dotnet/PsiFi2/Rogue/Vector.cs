// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Rogue
{
    /// <summary>
    /// Represents a cartesian vector with an ordered pair of horizontal and vertical values.
    /// </summary>
    [Serializable]
    public struct Vector : IEquatable<Vector>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref='Vector'/> class.
        /// </summary>
        public static readonly Vector Empty;

        public static readonly Vector NW = new Vector(-1, -1);
        public static readonly Vector N  = new Vector( 0, -1);
        public static readonly Vector NE = new Vector(+1, -1);
        public static readonly Vector W  = new Vector(-1,  0);
        public static readonly Vector E  = new Vector(+1,  0);
        public static readonly Vector SW = new Vector(-1, +1);
        public static readonly Vector S  = new Vector( 0, +1);
        public static readonly Vector SE = new Vector(+1, +1);

        private int i; // Do not rename (binary serialization)
        private int j; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='Vector'/> class from the specified
        /// <see cref='Point'/>.
        /// </summary>
        public Vector(Point point)
        {
            i = point.X;
            j = point.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Vector'/> class from the specified
        /// <see cref='Size'/>.
        /// </summary>
        public Vector(Size size)
        {
            i = size.Width;
            j = size.Height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='Vector'/> class from the specified dimensions.
        /// </summary>
        public Vector(int i, int j)
        {
            this.i = i;
            this.j = j;
        }

        /// <summary>
        /// Performs vector addition of two <see cref='Vector'/> objects.
        /// </summary>
        public static Vector operator +(Vector left, Vector right) => Add(left, right);

        /// <summary>
        /// Contracts a <see cref='Vector'/> by another <see cref='Vector'/>
        /// </summary>
        public static Vector operator -(Vector left, Vector right) => Subtract(left, right);

        /// <summary>
        /// Multiplies a <see cref="Vector"/> by an <see cref="int"/> producing <see cref="Vector"/>.
        /// </summary>
        /// <param name="multiplier">Multiplier of type <see cref="int"/>.</param>
        /// <param name="vector">Multiplicand of type <see cref="Vector"/>.</param>
        /// <returns>Product of type <see cref="Vector"/>.</returns>
        public static Vector operator *(int multiplier, Vector vector) => Multiply(vector, multiplier);

        /// <summary>
        /// Multiplies <see cref="Vector"/> by an <see cref="int"/> producing <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector">Multiplicand of type <see cref="Vector"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref="int"/>.</param>
        /// <returns>Product of type <see cref="Vector"/>.</returns>
        public static Vector operator *(Vector vector, int multiplier) => Multiply(vector, multiplier);

        /// <summary>
        /// Divides <see cref="Vector"/> by an <see cref="int"/> producing <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector">Dividend of type <see cref="Vector"/>.</param>
        /// <param name="divisor">Divisor of type <see cref="int"/>.</param>
        /// <returns>Result of type <see cref="Vector"/>.</returns>
        public static Vector operator /(Vector vector, int divisor) => new Vector(unchecked(vector.i / divisor), unchecked(vector.j / divisor));

        /// <summary>
        /// Tests whether two <see cref='Vector'/> objects are identical.
        /// </summary>
        public static bool operator ==(Vector left, Vector right) => left.I == right.I && left.J == right.J;

        /// <summary>
        /// Tests whether two <see cref='Vector'/> objects are different.
        /// </summary>
        public static bool operator !=(Vector left, Vector right) => !(left == right);

        /// <summary>
        /// Converts the specified <see cref='Vector'/> to a <see cref='Point'/>.
        /// </summary>
        public static explicit operator Point(Vector vector) => new Point(vector.I, vector.J);

        /// <summary>
        /// Converts the specified <see cref='Vector'/> to a <see cref='Point'/>.
        /// </summary>
        public static explicit operator Size(Vector vector) => new Size(vector.I, vector.J);

        /// <summary>
        /// Tests whether this <see cref='Vector'/> has zero magnitude.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => i == 0 && j == 0;

        /// <summary>
        /// Represents the horizontal component of this <see cref='Vector'/>.
        /// </summary>
        public int I
        {
            readonly get => i;
            set => i = value;
        }

        /// <summary>
        /// Represents the vertical component of this <see cref='Vector'/>.
        /// </summary>
        public int J
        {
            readonly get => j;
            set => j = value;
        }

        /// <summary>
        /// The the square of the radius of this point as a vector.
        /// </summary>
        public readonly int RadiusSquared => i * i + j * j;

        /// <summary>
        /// Performs vector addition of two <see cref='Vector'/> objects.
        /// </summary>
        public static Vector Add(Vector left, Vector right) =>
            new Vector(unchecked(left.I + right.I), unchecked(left.J + right.J));

        /// <summary>
        /// Contracts a <see cref='Vector'/> by another <see cref='Vector'/>.
        /// </summary>
        public static Vector Subtract(Vector left, Vector right) =>
            new Vector(unchecked(left.I - right.I), unchecked(left.J - right.J));

        /// <summary>
        /// Tests to see whether the specified object is a <see cref='Vector'/>  with the same dimensions
        /// as this <see cref='Vector'/>.
        /// </summary>
        public override readonly bool Equals(object? obj) => obj is Vector && Equals((Vector)obj);

        public readonly bool Equals(Vector other) => this == other;

        /// <summary>
        /// Returns a hash code.
        /// </summary>
        public override readonly int GetHashCode() => HashCode.Combine(I, J);

        /// <summary>
        /// Creates a human-readable string that represents this <see cref='Vector'/>.
        /// </summary>
        public override readonly string ToString() => "{DX=" + i.ToString() + ", DY=" + j.ToString() + "}";

        /// <summary>
        /// Multiplies <see cref="Vector"/> by an <see cref="int"/> producing <see cref="Vector"/>.
        /// </summary>
        /// <param name="vector">Multiplicand of type <see cref="Vector"/>.</param>
        /// <param name="multiplier">Multiplier of type <see cref='int'/>.</param>
        /// <returns>Product of type <see cref="Size"/>.</returns>
        private static Vector Multiply(Vector vector, int multiplier) =>
            new Vector(unchecked(vector.i * multiplier), unchecked(vector.j * multiplier));

        /// <summary>
        /// Returns a list of vectors formed by reflecting this <see cref="Vector"/> over the two
        /// cardinal axes.
        /// </summary>
        /// <returns>
        /// One, two or four vectors that are reflections on this <see cref="Vector"/>, including
        /// this <see cref="Vector"/>.
        /// </returns>
        public readonly IEnumerable<Vector> MirroredAcrossCardinalAxes()
        {
            yield return this;
            if (I != 0) yield return new Vector(-I, +J);
            if (J != 0)
            {
                yield return new Vector(+I, -J);
                if (I != 0) yield return new Vector(-I, -J);
            }
        }

        /// <summary>
        /// Returns a list of vectors formed by reflecting this <see cref="Vector"/> over the two
        /// 45° diagonal axes.
        /// </summary>
        /// <returns>
        /// One, two or four vectors that are reflections on this <see cref="Vector"/>, including
        /// this <see cref="Vector"/>.
        /// </returns>
        public readonly IEnumerable<Vector> MirroredAcrossDiagonalAxes()
        {
            yield return this;
            if (IsEmpty) yield break;
            yield return new Vector(-I, -J);
            if (Math.Abs(I) != Math.Abs(J))
            {
                yield return new Vector(+J, +I);
                yield return new Vector(-J, -I);
            }
        }

        /// <summary>
        /// Returns a list of vectors formed by reflecting this <see cref="Vector"/> over the two
        /// cardinal and two 45° diagonal axes.
        /// </summary>
        /// <returns>
        /// One, two, four or eight vectors that are reflections on this <see cref="Vector"/>,
        /// including this <see cref="Vector"/>.
        /// </returns>
        public readonly IEnumerable<Vector> MirroredAcrossCardinalAndDiagonalAxes()
        {
            if (IsEmpty)
                return new Vector[1] { Empty };
            else if (I == 0 || J == 0)
                return MirroredAcrossDiagonalAxes();
            else if (I == J)
                return MirroredAcrossCardinalAxes();
            else
                return Enumerable.Concat(
                    MirroredAcrossCardinalAxes(),
                    new Vector(J, I).MirroredAcrossCardinalAxes()
                );
        }
    }
}