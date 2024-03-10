using System.Drawing;

namespace FiveStar
{
    public interface IProjector
    {
        /// <summary>
        /// Projects the specified point onto a flat surface.
        /// </summary>
        /// <param name="s">The <see cref="SphericalPoint"/> to project.</param>
        /// <returns>The <see cref="NamedPoint"/> where <paramref name="s"/> is shown on a flat surface.</returns>
        NamedPoint Project(SphericalPoint s);
    }
}