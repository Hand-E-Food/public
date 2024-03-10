using System;
using System.Collections.Generic;

namespace System.Collections.Generic
{

    /// <summary>
    /// Provides a base class for implementations of the System.Collections.Generic.IComparer<T> generic
    /// interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public class ReversedComparer<T> : Comparer<T> where T : IComparable<T>
    {

        /// <summary>
        /// Performs a comparison of two objects of the same type and returns a value indicating whether one
        /// object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of x and y.</returns>
        public override int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}
