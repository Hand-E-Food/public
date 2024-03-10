namespace RandomVectorMap
{

    /// <summary>
    /// Represents a repetative task that can be stepped through.
    /// </summary>
    public interface IStepper
    {

        /// <summary>
        /// Initialises the class after properties have been set.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets a value indicating whether this stepper has finished its task.
        /// </summary>
        /// <value>True if this stepper has finished its task; otherwise, false.</value>
        bool IsFinished { get; }

        /// <summary>
        /// Gets a value indicating whether this stepper has been initialised.
        /// </summary>
        /// <value>True if this stepper has been initialised; otherwise, false.</value>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets this stepper's name.
        /// </summary>
        /// <value>This stepper's name.</value>
        string Name { get; }

        /// <summary>
        /// Performs a single step of its task.
        /// </summary>
        void Step();
    }
}
