using System.Collections.Generic;

namespace Rogue.ConsoleForms
{
    public static class StringExtensions
    {
        public static string Align(this string text, HorizontalAlignment horizontalAlignment, int width)
        {
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left: return text.AlignLeft(width);
                case HorizontalAlignment.Right: return text.AlignRight(width);
                case HorizontalAlignment.Center: return text.AlignCenter(width);
                default: return text.AlignLeft(width);
            }
        }

        public static string AlignCenter(this string text, int width)
        {
            if (width <= 0)
                return string.Empty;

            var space = width - text.Length;
            if (space >= 0)
            {
                space /= 2;
                return (new string(' ', space) + text).PadRight(width);
            }
            else
            {
                return text.Remove(width - 1) + "…";
            }
        }

        public static string AlignLeft(this string text, int width)
        {
            if (width <= 0)
                return string.Empty;
            else if (text.Length <= width)
                return text.PadRight(width);
            else
                return text.Remove(width - 1) + "…";
        }

        public static string AlignRight(this string text, int width)
        {
            if (width <= 0)
                return string.Empty;
            else if (text.Length <= width)
                return text.PadLeft(width);
            else
                return "…" + text.Substring(text.Length - width + 1);
        }

        public static IEnumerable<string> Wrap(this string text, int width)
        {
            int start = 0;
            while (text.Length - start > width)
            {
                var end = text.LastIndexOf(' ', start + width + 1);
                if (end < start)
                    end = start + width;

                yield return text.Substring(start, end - start);
                
                start = end;
                while (text[start] == ' ')
                    start++;
            }
            yield return text.Substring(start);
        }

    }
}
