namespace StarCoder.Models;

/// <summary>
/// A project to learn a new language.
/// </summary>
/// <param name="quantity">The quantity of understanding required to learn the language.</param>
/// <param name="language">The language to learn.</param>
/// <param name="roots">The new language's roots. Using these languages helps to learn the new language faster.</param>
public class EducationProject(int quantity, Language language, params Language[] roots) : Project, IProject
{
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

    public override void Produce(FeatureProduction production)
    {
        base.Produce(production);
        QuantityProduced += Roots.Contains(production.Language) ? 2 : 1;
        if (QuantityProduced >= QuantityRequired)
        {
            QuantityProduced = QuantityRequired;
            State = ProjectState.Completed;
        }
    }
}
