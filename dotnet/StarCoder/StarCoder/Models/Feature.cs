namespace StarCoder.Models;

/// <summary>
/// An application feature.
/// </summary>
public sealed class Feature
{
    public static readonly Feature Audio = new("Aud", "Audio");
    public static readonly Feature Database = new("DB", "Database");
    public static readonly Feature Graphics = new("Gfx", "Graphics");
    public static readonly Feature Logic = new("Lgc", "Logic");
    public static readonly Feature MachineLearning = new("ML", "Machine Learning");
    public static readonly Feature Networking = new("Net", "Networking");
    public static readonly Feature Performance = new("Prf", "Performance");
    public static readonly Feature Quantum = new("QC", "Quantum Computing");
    public static readonly Feature Security = new("Sec", "Security");
    public static readonly Feature UserInterface = new("UI", "User Interface");

    private Feature(string abbreviation, string name)
    {
        Abbreviation = abbreviation;
        Name = name;
    }

    /// <summary>
    /// This feature's abbreviation.
    /// </summary>
    public string Abbreviation { get; }

    /// <summary>
    /// This feature's name.
    /// </summary>
    public string Name { get; }

    public override string ToString() => Name;
}
