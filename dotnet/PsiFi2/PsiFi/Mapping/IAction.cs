using PsiFi.ConsoleForms;

namespace PsiFi.Mapping
{
    /// <summary>
    /// An actor's action.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <param name="mapScreen">The <see cref="MapScreen"/> to perform upon.</param>
        void Perform(MapScreen mapScreen);
    }
}
