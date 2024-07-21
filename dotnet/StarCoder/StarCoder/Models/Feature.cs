namespace StarCoder.Models;

/// <summary>
/// An application feature.
/// </summary>
public sealed class Feature
{
    public static readonly Feature Any = new("* Any *");
    public static readonly Feature Audio = new("Audio");
    public static readonly Feature Database = new("Database");
    public static readonly Feature Graphics = new("Graphics");
    public static readonly Feature Logic = new("Logic");
    public static readonly Feature MachineLearning = new("Machine Learning");
    public static readonly Feature Networking = new("Networking");
    public static readonly Feature Performance = new("Performance");
    public static readonly Feature Security = new("Security");
    public static readonly Feature UserInterface = new("User Interface");

    private readonly string name;
    private Feature(string name)
    {
        this.name = name;
    }
    public override string ToString() => name;
}
