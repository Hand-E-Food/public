using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Generates a map.
/// </summary>
public partial class MapGenerator : IStepper
{
    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value> 
    public bool IsFinished => stepper.IsFinished;

    /// <summary>
    /// Gets a value indicating whether this stepper has been initialised.
    /// </summary>
    /// <value>True if this stepper has been initialised; otherwise, false.</value>
    public bool IsInitialized { get; private set; } = false;

    /// <summary>
    /// Gets the map being generated.
    /// </summary>
    /// <value>The map being generated.</value>
    public Map Map { get; } = new();

    /// <summary>
    /// Gets or sets this stepper's name.
    /// </summary>
    /// <value>This stepper's name.</value>
    public required string Name { get; init; }

    /// <summary>
    /// Gets or sets the random number generator to use.
    /// </summary>
    public Random Random { get; set; } = new();

    /// <summary>
    /// The stepper controller.
    /// </summary>
    private readonly StepperCollection stepper = new();

    /// <summary>
    /// Gets the name of the current task.
    /// </summary>
    /// <value>The current task's name. Null if all tasks are finished.</value>
    public string? TaskName => stepper.Tasks.FirstOrDefault(t => !t.IsFinished)?.Name;

    /// <summary>
    /// Adds a task to the generator.
    /// </summary>
    /// <param name="task">The task to add.</param>
    public void AddTask(IMapGenerator task)
    {
        task.Map = Map;
        task.Random = Random;
        stepper.Tasks.Add(task);
    }

    /// <summary>
    /// Finishes the current task.
    /// </summary>
    public void FinishTask()
    {
        Map.ClearDebug();
        stepper.FinishTask();
    }

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public virtual void Initialize()
    {
        stepper.Initialize();
        IsInitialized = true;
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public void Step()
    {
        Map.ClearDebug();
        stepper.Step();
    }
}
