namespace RandomVectorMap;

/// <summary>
/// A collection of steppable tasks.
/// </summary>
public class StepperCollection : IStepper
{
    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value> 
    public bool IsFinished { get; private set; } = false;

    /// <summary>
    /// Gets a value indicating whether this stepper has been initialised.
    /// </summary>
    /// <value>True if this stepper has been initialised; otherwise, false.</value>
    public bool IsInitialized { get; private set; } = false;

    /// <summary>
    /// Gets or sets this stepper's name.
    /// </summary>
    /// <value>This stepper's name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets the collection of steppable tasks to perform.
    /// </summary>
    /// <value>A collection of steppable tasks.</value>
    public List<IStepper> Tasks { get; private set; } = [];

    /// <summary>
    /// Finishes all tasks.
    /// </summary>
    public void Finish()
    {
        while (!IsFinished)
            FinishTask();
    }

    /// <summary>
    /// Finishes the current task.
    /// </summary>
    public void FinishTask()
    {
        var task = GetNextTask();
        while (!task.IsFinished)
            task.Step();
    }

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public virtual void Initialize()
    {
        IsInitialized = true;
    }

    /// <summary>
    /// Performs a single step of its task.
    /// </summary>
    public void Step() => GetNextTask().Step();

    /// <summary>
    /// Gets the next steppable task and ensures it is initialised.
    /// If there are no more tasks, sets <see cref="IsFinished"/> to <c>true</c>.
    /// </summary>
    /// <returns>The steppable task.</returns>
    private IStepper GetNextTask()
    {
        using var tasks = Tasks.GetEnumerator();

        while(tasks.MoveNext())
        {
            var task = tasks.Current;
            if (!task.IsInitialized) task.Initialize();
            if (!task.IsFinished) return task;
        }

        IsFinished = true;
        return EmptyStepper.Instance;
    }
}
