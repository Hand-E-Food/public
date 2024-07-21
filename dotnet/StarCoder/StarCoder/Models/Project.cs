namespace StarCoder.Models;

/// <summary>
/// A project to be completed.
/// </summary>
public interface IProject
{
    /// <summary>
    /// This project's name.
    /// </summary>
    string Name { get; }
}

/// <summary>
/// Common methods for projects.
/// </summary>
public abstract class Project
{
    /// <summary>
    /// This project's current state.
    /// </summary>
    public ProjectState State { get; protected set; }

    /// <summary>
    /// The coder accepts and signs the project contract.
    /// </summary>
    /// <exception cref="InvalidOperationException">The project is in an invalid state.</exception>
    public virtual void Accept()
    {
        if (State != ProjectState.Offered)
            throw new InvalidOperationException($"Cannot accept this project because it is {State}.");
        State = ProjectState.Accepted;
    }

    /// <summary>
    /// The coder produces some of this project's features.
    /// </summary>
    /// <param name="production">The feature to produce.</param>
    /// <exception cref="InvalidOperationException">The project is in an invalid state.</exception>
    public virtual void Produce(FeatureProduction production)
    {
        if (State != ProjectState.Accepted)
            throw new InvalidOperationException($"Cannot produce this project because it is {State}.");
    }
}
