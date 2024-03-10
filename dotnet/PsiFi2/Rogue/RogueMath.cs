using static System.Math;

namespace Rogue
{
    public static class RogueMath
    {
        /// <summary>
        /// Gets the square of radius + 0.5.
        /// </summary>
        /// <param name="radius">The radius to square.</param>
        /// <returns>The squared radius.</returns>
        public static int RadiusSquared(int radius) => (int)Floor(Pow(radius + 0.5, 2.0));
    }
}
