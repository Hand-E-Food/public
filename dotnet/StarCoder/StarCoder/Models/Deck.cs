namespace StarCoder.Models;

public class Deck<T>(int deckCapacity, int handCapacity)
{
    private readonly Random random = new();

    /// <summary>
    /// The discarded items.
    /// </summary>
    public List<T> DiscardPile { get; } = new(deckCapacity);

    /// <summary>
    /// The items to be drawn.
    /// </summary>
    public List<T> DrawPile { get; } = new(deckCapacity);

    /// <summary>
    /// This items in hand.
    /// </summary>
    public List<T> Hand { get; } = new(handCapacity);

    /// <summary>
    /// Draws items from the draw pile into the hand.
    /// </summary>
    /// <param name="count">The number of items to draw.</param>
    public void Draw(int count)
    {
        while (count > 0)
        {
            count--;
            if (DrawPile.Count == 0)
            {
                if (DiscardPile.Count == 0) return;
                RecycleDiscardPile();
            }
            int i = DrawPile.Count - 1;
            Hand.Add(DrawPile[i]);
            DrawPile.RemoveAt(i);
        }
    }

    /// <summary>
    /// Puts the discard pile in the draw pile and shuffles it.
    /// </summary>
    public void RecycleDiscardPile()
    {
        DrawPile.AddRange(DiscardPile);
        DiscardPile.Clear();
        ShuffleDrawPile();
    }

    /// <summary>
    /// Shuffles the draw pile.
    /// </summary>
    public void ShuffleDrawPile()
    {
        for (int i = DrawPile.Count; i > 1; )
        {
            int j = random.Next(i--);
            (DrawPile[i], DrawPile[j]) = (DrawPile[j], DrawPile[i]);
        }
    }
}
