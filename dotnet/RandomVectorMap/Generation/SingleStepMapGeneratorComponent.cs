namespace RandomVectorMap.Generation;

/// <summary>
/// Contains the framework for a map generator component that only performs a single step.
/// </summary>
public abstract class SingleStepMapGeneratorComponent : MapGeneratorComponent
{
    /// <summary>
    /// Gets a value indicating whether this stepper has finished its task.
    /// </summary>
    /// <value>True if this stepper has finished its task; otherwise, false.</value>
    public override bool IsFinished => IsInitialized && isFinished;
    private bool isFinished = false;

    /// <summary>
    /// Performs a single step of map generation.
    /// </summary>
    public override void Step()
    {
        isFinished = true;
    }
}
