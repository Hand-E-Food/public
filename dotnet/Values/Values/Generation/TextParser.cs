using Microsoft.Maui.Graphics;
using System.Text;
using Values.Model;

namespace Values.Generation;

/// <summary>
/// Transforms text into a <see cref="Block"/> object.
/// </summary>
public interface ITextParser
{
    /// <summary>
    /// Creates a block of text from a formatted string.
    /// </summary>
    /// <param name="text">The formatted string to parse.</param>
    /// <returns>A block of text.</returns>
    /// <remarks>
    /// Use <c>|</c> to separate table cells. Include a <c>|</c> before the first and after the last cell in each row.
    /// Use <c>\n</c> to insert a new line, including separating table rows.
    /// Use the values in <see cref="Model.Category"/>, <see cref="Model.Resource"/> or <see cref="Model.Symbol"/> to
    /// insert an icon.
    /// </remarks>
    Block CreateBlock(string text);
}

/// <inheritdoc cref="ITextParser"/>
public class TextParser : ITextParser
{
    private enum WordState { String, Image }

    public Block CreateBlock(string input)
    {
        List<Paragraph> paragraphs = [];
        List<string> tableLines = [];

        void AddLinearParagraph(string line)
        {
            LinearParagraph paragraph = CreateLinearParagraph(line);
            if (paragraph.Words.Count == 0) return;
            paragraphs.Add(paragraph);
        }

        void AddTableParagraph()
        {
            if (tableLines.Count == 0) return;
            Paragraph paragraph = CreateTableParagraph(tableLines);
            tableLines.Clear();
            if (paragraph.Words.Count == 0) return;
            paragraphs.Add(paragraph);
        }

        foreach (string line in input.Split('\n'))
        {
            if (line.Contains('\t'))
            {
                tableLines.Add(line);
            }
            else
            {
                AddTableParagraph();
                AddLinearParagraph(line);
            }
        }
        AddTableParagraph();
        return new(paragraphs);
    }

    private LinearParagraph CreateLinearParagraph(string input)
    {
        return new(input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(CreateWord).ToArray());
    }

    private TableParagraph CreateTableParagraph(IEnumerable<string> lines)
    {
        throw new NotImplementedException(); //TODO
    }

    private Word CreateWord(string input)
    {
        List<Fragment> fragments = [];
        WordState state = WordState.String;
        StringBuilder token = new(input.Length);
        var enumerator = input.GetEnumerator();

        void AddStringFragment()
        {
            if (token.Length == 0) return;
            string text = token.ToString();
            fragments.Add(new StringFragment(text));
            token.Clear();
        }

        void AddImageFragment()
        {
            if (token.Length == 0) throw new FormatException("Empty image token.");
            string name = token.ToString();
            //IImage getImage(float height) => GetImage(name, height);
            //fragments.Add(new ImageFragment(getImage));
            //***** Temporary code
            Dictionary<string, string> images = new() {
                { Category.Beauty, "🌺" },
                { Category.Career, "🏢" },
                { Category.Charity, "🎗️" },
                { Category.Creativity, "🎨" },
                { Category.Faith, "🙏" },
                { Category.Family, "👥" },
                { Category.Justice, "⚖️" },
                { Category.Pleasure, "🍷" },
                { Category.Status, "💎" },
                { Resource.E, "😊" },
                { Resource.F, "💰" },
                { Resource.M, "🧩" },
                { Resource.P, "🍎" },
                { Resource.T, "⏳" },
                { Symbol.Cost, "🔽" },
                { Symbol.Exclusive, "1️⃣" },
                { Symbol.Gain, "🔼" },
                { Symbol.Point, "⭐️" },
            };
            fragments.Add(new StringFragment(images[name]));
            //*****
            token.Clear();
        }

        while (enumerator.MoveNext())
        {
            switch (enumerator.Current)
            {
                case '[':
                    if (state != WordState.String) throw new FormatException("Nested image token.");
                    AddStringFragment();
                    state = WordState.Image;
                    break;
                case ']':
                    if (state != WordState.Image) throw new FormatException("Image token was closed without being opened.");
                    AddImageFragment();
                    state = WordState.String;
                    break;
                default:
                    token.Append(enumerator.Current);
                    break;
            }
        }
        if (state == WordState.Image) throw new FormatException("Image token was opened without being closed.");
        AddStringFragment();

        return new(fragments);
    }

    private IImage GetImage(string name, float height)
    {
        throw new NotImplementedException(); //TODO
    }
}
