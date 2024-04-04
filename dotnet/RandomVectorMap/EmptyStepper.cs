namespace RandomVectorMap;

/// <summary>
/// Implements <see cref="IStepper"/> and does nothing.
/// </summary>
public class EmptyStepper : IStepper
{
    /// <summary>
    /// A static instance of the <see cref="EmptyStepper"/> class.
    /// </summary>
    public static readonly EmptyStepper Instance = new();

    private EmptyStepper() { }

    public bool IsFinished => true;

    public bool IsInitialized => true;

    public string Name => "No step";

    public void Initialize() { }

    public void Step() { }
}
