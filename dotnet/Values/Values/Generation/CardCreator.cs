using System.Text.RegularExpressions;
using Values.Model;

namespace Values.Generation;

/// <summary>
/// Draws and saves card images.
/// </summary>
public interface ICardCreator
{
    /// <summary>
    /// Draws and saves the card image.
    /// </summary>
    /// <param name="card">The card to create.</param>
    public void Create(Background card);

    /// <inheritdoc cref="Create(Background)"/>
    public void Create(Decade card);

    /// <inheritdoc cref="Create(Background)"/>
    public void Create(Lifestyle card);

    /// <inheritdoc cref="Create(Background)"/>
    public void Create(Value card);
}

/// <param name="cardDrawer">The instance that draws cards to a canvas.</param>
/// <inheritdoc cref="ICardCreator"/>
public class CardCreator(ICardDrawer cardDrawer) : ICardCreator
{
    private const string fileExtension = ".png";

    public void Create(Background card)
    {
        CardPng image = new();
        cardDrawer.DrawTo(image.Canvas, card);
        string fileName = NormaliseFileName(card.Title);
        string path = Path.Combine("backgrounds", fileName + fileExtension);
        image.WriteToFile(path);
    }

    public void Create(Decade card)
    {
        CardPng image = new();
        cardDrawer.DrawTo(image.Canvas, card);
        string fileName = NormaliseFileName(card.Title);
        string path = Path.Combine("decades", fileName + fileExtension);
        image.WriteToFile(path);
    }

    public void Create(Lifestyle card)
    {
        CardPng image = new();
        cardDrawer.DrawTo(image.Canvas, card);
        string decade = NormaliseFileName(card.Decade);
        string fileName = NormaliseFileName(card.Title);
        if (card.Subtitle is not null) fileName += "-" + NormaliseFileName(card.Subtitle);
        string path = Path.Combine("lifestyles", decade, fileName + fileExtension);
        image.WriteToFile(path);
    }

    public void Create(Value card)
    {
        CardPng image = new();
        cardDrawer.DrawTo(image.Canvas, card);
        string fileName = NormaliseFileName(card.Title);
        string path = Path.Combine("values", fileName + fileExtension);
        image.WriteToFile(path);
    }

    private static string NormaliseFileName(string fileName)
    {
        fileName = NonAlphanumericRegex().Replace(fileName, "");
        return fileName;
    }

    [GeneratedRegex(@"[^0-9A-Za-z]")]
    private static partial Regex NonAlphanumericRegex();
}
