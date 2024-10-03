using StarCoder.Models;

namespace StarCoder.View;

public interface IMainScreen
{
    void Focus();
    void DrawBurnout(int burnout);
    void DrawHandCards(IList<Language> cards);
    void DrawHandCard(int position, Language? card, bool isSelected = false);
    void DrawMessage(string message);
    void DrawProjects(IList<Project> projects);
    void DrawProject(int position, Project? projects, bool isSelected = false);
    void DrawWeek(int week);
}

public class MainScreen(GameSettings settings) : IMainScreen
{
    private static readonly char[] CardKeys = "1234567890".ToCharArray();
    private const int CardWidth = 10;
    private readonly GameSettings settings = settings;

    public void Focus()
    {
        int width = settings.MaximumHandSize * CardWidth;
        Console.CursorVisible = false;
        Console.SetWindowSize(width, 25);
        Console.SetBufferSize(width, 26);
        Console.Clear();
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

    public void DrawHandCards(IList<Language> cards)
    {
        int position;
        for (position = 0; position < cards.Count; position++)
            DrawHandCard(position, cards[position]);
        for (; position < settings.MaximumHandSize; position++)
            DrawHandCard(position, null);
    }

    public void DrawHandCard(int position, Language? card, bool isSelected = false)
    {
        int x = position * CardWidth;
        if (card is null)
        {
            for (int y = 18; y < 24; y++)
            {
                Console.SetCursorPosition(position * CardWidth, y);
                Console.Write("          ");
            }
        }
        else
        {
            string key = isSelected ? "vv" : $"#{CardKeys[position]}";
            Console.SetCursorPosition(x, 18);
            Console.Write($"┌───{key}───┐");
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
    }

    public void DrawMessage(string message)
    {
        Console.SetCursorPosition(0, 1);
        Console.Write(message.PadRight(Console.BufferWidth));
    }

    public void DrawProjects(IList<Project> projects)
    {
        int position;
        for (position = 0; position < projects.Count; position++)
            DrawProject(position, projects[position]);
        for (; position < settings.MaximumProjectCount; position++)
            DrawProject(position, null);
    }

    public void DrawProject(int position, Project? projects, bool isSelected = false)
    { }

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
