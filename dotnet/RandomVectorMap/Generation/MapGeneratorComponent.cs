using RandomVectorMap.Mapping;

namespace RandomVectorMap.Generation;

/// <summary>
/// Concretes common features of map generation components.
/// </summary>
public abstract class MapGeneratorComponent : IMapGenerator
{
    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public abstract bool IsFinished { get; }

    /// <summary>
    /// Gets a value indicating whether this stepper has been initialised.
    /// </summary>
    /// <value>True if this stepper has been initialised; otherwise, false.</value>
    public bool IsInitialized { get; private set; } = false;

    /// <summary>
    /// The map to generate.
    /// </summary>
    public Map Map
    {
        get
        {
            if (map is null) throw new InvalidOperationException($"{nameof(Map)} has not been set.");
            return map;
        }
        set => map = value;
    }
    private Map? map = null;

    /// <summary>
    /// Gets or sets this component's name.
    /// </summary>
    /// <value>This component's name.</value>
    public string Name { get; set; } = "Generating something...";

    /// <summary>
    /// Gets or sets the random number generator to use.
    /// </summary>
    public Random Random
    {
        get
        {
            if (random is null) throw new InvalidOperationException($"{nameof(Random)} has not been set.");
            return random;
        }
        set => random = value;
    }
    private Random? random = null;

    /// <summary>
    /// Initialises the class after properties have been set.
    /// </summary>
    public virtual void Initialize()
    {
        if (IsInitialized) throw new InvalidOperationException("This class is already initialized.");
        IsInitialized = true;
    }

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public abstract void Step();
}
