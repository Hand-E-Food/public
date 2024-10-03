namespace StarCoder.Models;

/// <summary>
/// A project to learn a new language.
/// </summary>
/// <param name="quantity">The quantity of understanding required to learn the language.</param>
/// <param name="language">The language to learn.</param>
public class EducationProject(int quantity, Language language) : Project, IProject
{
    public override ProjectOutcome GetOutcome()
    {
        if (State == ProjectState.Completed)
            return new() { Language = Language };
        else
            return ProjectOutcome.None;
    }

    /// <summary>
    /// The language to learn.
    /// </summary>
    public Language Language { get; } = language;

    public string Name => $"Learn {Language.Name}";

    /// <summary>
    /// The quantity of understanding produced to learn the language.
    /// </summary>
    public int QuantityProduced { get; private set; } = 0;

    /// <summary>
    /// The quantity of understanding required to learn the language.
    /// </summary>
    public int QuantityRequired { get; } = quantity;

    /// <summary>
    /// The languages that help to learn the new language faster.
    /// </summary>
    public ICollection<Language> Roots { get; } = [language, .. language.Roots];

    public override void Produce(Language language, FeatureProduction production)
    {
        base.Produce(language, production);
        QuantityProduced += Roots.Contains(language) ? 2 : 1;
        if (QuantityProduced >= QuantityRequired)
        {
            QuantityProduced = QuantityRequired;
            State = ProjectState.Completed;
        }
    }
}
