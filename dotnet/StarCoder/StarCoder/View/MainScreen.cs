using StarCoder.Models;

namespace StarCoder.View;

public interface IMainScreen
{
    void Focus();
    void DrawBurnout(int burnout);
    void DrawHand(IList<Language> cards);
    void DrawMessage(string message);
    void DrawWeek(int week);
}

public class MainScreen : IMainScreen
{
    public void Focus()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(100, 25);
        Console.SetBufferSize(100, 26);
        Console.Clear();
        Console.SetWindowPosition(0, 0);
    }

    public void DrawBurnout(int burnout)
    {
        string mood;
        if (burnout < 6)
            mood = "Relaxed ";
        else if (burnout < 12)
            mood = "Occupied";
        else if (burnout < 18)
            mood = "Stressed";
        else if (burnout < 24)
            mood = "Burnout!";
        else
            mood = "I QUIT!!";
        
        Console.SetCursorPosition(20, 0);
        Console.Write(mood);
    }

    public void DrawHand(IList<Language> cards)
    {
        const int MaxCards = 10;
        const int CardWidth = 10;
        int i;
        for (i = 0; i < cards.Count; i++)
        {
            int x = i * CardWidth;
            char key = "1234567890"[i];
            var card = cards[i];
            Console.SetCursorPosition(x, 18);
            Console.Write($"┌───#{key}───┐");
            Console.SetCursorPosition(x, 19);
            Console.Write($"│{AlignCentre(8, card.Abbreviation)}│");
            int y;
            for (y = 0; y < card.Production.Count; y++)
            {
                var production = card.Production[y];
                Console.SetCursorPosition(x, 20 + y);
                Console.Write($"│{production.Feature.Abbreviation:-4}{production.Quantity:N0,4}│");
            }
            for (; y < 5; y++)
            {
                Console.SetCursorPosition(x, 20 + y);
                Console.Write("│        │");
            }
        }
        if (i < MaxCards)
        {
            string padding = new string(' ', (MaxCards - i) * CardWidth);
            for (int y = 18; y < 24; y++)
            {
                Console.SetCursorPosition(i * CardWidth, y);
                Console.Write(padding);
            }
        }
    }

    public void DrawMessage(string message)
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(message.PadRight(Console.BufferWidth));
    }

    public void DrawWeek(int week)
    {
        int year = week / 52 + 20;
        week = week % 52 + 1;
        Console.SetCursorPosition(0, 0);
        Console.Write($"Age {year}, Week {week} ");
    }

    private static string AlignCentre(int width, string text) =>
        text.PadLeft((width - text.Length) / 2).PadRight(width);
}
