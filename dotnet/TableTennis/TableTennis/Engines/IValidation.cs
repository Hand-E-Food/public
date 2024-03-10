namespace TableTennis.Engines
{
    /// <summary>
    /// Validates a set of inputs.
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// Validates the inputs associated with this validator.
        /// </summary>
        /// <returns>True if the inputs are valid; otherwise, false.</returns>
        bool IsValid();
    }
}