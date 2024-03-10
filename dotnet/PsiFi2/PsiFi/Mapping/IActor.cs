using PsiFi.Mapping.Actions;

namespace PsiFi.Mapping
{
    /// <summary>
    /// An entity that acts in turn.
    /// </summary>
    public interface IActor
    {
        /// <summary>
        /// The time before this actor's next turn measured in milliseconds.
        /// </summary>
        int TimeUntilNextTurn { get; }

        /// <summary>
        /// Lets this actor take its turn.
        /// </summary>
        /// <returns>The result of this actor's action.</returns>
        IAction Act();

        /// <summary>
        /// Informs this actor that time has passed.
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds that have passed.</param>
        /// <remarks>
        /// Typically, this will just decrement <see cref="TimeUntilNextTurn"/>, but may cause
        /// other effects.
        /// </remarks>
        void PassTime(int milliseconds);
    }
}
