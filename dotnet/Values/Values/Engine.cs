using Values.Generation;

namespace Values;

/// <summary>
/// Performs all automation.
/// </summary>
public interface IEngine
{
    /// <summary>
    /// Runs the automation.
    /// </summary>
    void Run();
}

/// <param name="cardCreator">The instance that creates the cards.</param>
/// <param name="cardRepository">The repository of all cards.</param>
/// <inheritdoc cref="IEngine"/>
public class Engine(ICardCreator cardCreator, ICardRepository cardRepository) : IEngine
{
    public void Run()
    {
        foreach(var card in cardRepository.Backgrounds)
            cardCreator.Create(card);
        foreach(var card in cardRepository.Decades)
            cardCreator.Create(card);
        foreach(var card in cardRepository.Lifestyles)
            cardCreator.Create(card);
        foreach(var card in cardRepository.Values)
            cardCreator.Create(card);
    }
}
