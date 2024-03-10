using Rogue;

namespace PsiFi.Mapping
{
    public interface IPhysical
    {

        /// <summary>
        /// This object's visual appearance.
        /// </summary>
        char Character { get; }

        /// <summary>
        /// This object's colour.
        /// </summary>
        Color ForeColor { get; }
    }
}
