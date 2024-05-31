using Microsoft.Maui.Graphics;
using System.Text;
using Values.Model;

namespace Values.Generation;

/// <summary>
/// Draws card images to an existing canvas.
/// </summary>
public interface ICardDrawer
{
    /// <summary>
    /// Draws the card data to the canvas.
    /// </summary>
    /// <param name="canvas">The canvas to draw to.</param>
    /// <param name="card">The card to draw.</param>
    public void DrawTo(ICanvas canvas, Background card);

    /// <inheritdoc cref="DrawTo(ICanvas, Background)"/>
    public void DrawTo(ICanvas canvas, Decade card);

    /// <inheritdoc cref="DrawTo(ICanvas, Background)"/>
    public void DrawTo(ICanvas canvas, Lifestyle card);

    /// <inheritdoc cref="DrawTo(ICanvas, Background)"/>
    public void DrawTo(ICanvas canvas, Value card);
}

/// <inheritdoc cref="ICardDrawer"/>
public class CardDrawer : ICardDrawer
{
    /// <summary>
    /// The vertical space between blocks.
    /// </summary>
    private const float BlockSpacing = 5f;

    /// <summary>
    /// The style for each block.
    /// </summary>
    private static class BlockStyles
    {
        public static readonly BlockStyle Title = new() {
            Font = new Font("Arial"),
            FontSize = 16,
            BlockHeight = 16,
        };

        public static readonly BlockStyle Icons = new() {
            Font = Title.Font,
            FontSize = Title.FontSize,
        };

        public static readonly BlockStyle Subtitle = new() {
            Font = new Font("Arial"),
            FontSize = 13,
            BlockHeight = 13,
        };

        public static readonly BlockStyle Body = new() {
            Font = new Font("Arial"),
            FontSize = 10,
            SpacingBefore = BlockSpacing,
        };
    }
    
    /// <summary>
    /// The margin around the edge of the card.
    /// </summary>
    private const float Margin = 5f;

    /// <summary>
    /// The text parser used to convert formatted text into blocks.
    /// </summary>
    private readonly TextParser textParser;

    public CardDrawer(TextParser textParser)
    {
        this.textParser = textParser;
    }

    /// <summary>
    /// Draws the specified card to the canvas.
    /// </summary>
    /// <param name="canvas">The canvas to draw to.</param>
    /// <param name="card">The card to draw.</param>
    public void DrawTo(ICanvas canvas, Background card)
    {
        RectF bounds = InitialiseBounds();
        bounds = DrawText(card.Title, BlockStyles.Title, bounds, canvas);
        bounds = DrawText(FormatBackgroundItems(card.Gains), BlockStyles.Body, bounds, canvas);
    }

    private string FormatBackgroundItems(Item[] gains)
    {
        StringBuilder text = new(7 + Resource.MaxLength + 16 * gains.Length);
        text.Append("||");
        text.Append(Resource.T);
        text.Append('|');
        text.Append(Resource.F);
        text.Append('|');
        text.Append(Resource.P);
        text.Append('|');
        text.Append(Resource.M);
        text.Append('|');
        text.Append(Resource.E);
        text.Append('|');
        foreach (Item gain in gains)
        {
            text.Append("\n|");
            text.Append(gain.D);
            text.Append('|');
            text.Append(gain.T);
            text.Append('|');
            text.Append(gain.F);
            text.Append('|');
            text.Append(gain.P);
            text.Append('|');
            text.Append(gain.M);
            text.Append('|');
            text.Append(gain.E);
            text.Append('|');
        }
        return text.ToString();
    }

    /// <inheritdoc cref="DrawTo(ICanvas, Background)"/>
    public void DrawTo(ICanvas canvas, Decade card)
    {
        RectF bounds = InitialiseBounds();
        bounds = DrawText(card.Title, BlockStyles.Title, bounds, canvas);
        bounds = DrawText(card.Text, BlockStyles.Body, bounds, canvas);
    }

    /// <inheritdoc cref="DrawTo(ICanvas, Background)"/>
    public void DrawTo(ICanvas canvas, Lifestyle card)
    {
        RectF availableBounds = InitialiseBounds();
        if (card.Exclusive) DrawExclusive(availableBounds, canvas);
        DrawCategories(card.Categories, availableBounds, canvas);
        availableBounds = DrawText(card.Title, BlockStyles.Title, availableBounds, canvas);
        if (card.Subtitle is not null) availableBounds = DrawText(card.Subtitle, BlockStyles.Subtitle, availableBounds, canvas);
        if (card.AbandonmentCost is not null) availableBounds = DrawAbandonmentCost(card.AbandonmentCost, availableBounds, canvas);
        availableBounds = DrawHorizontalLine(BlockStyles.Body.SpacingBefore, availableBounds, canvas);
        availableBounds = DrawGainOrCost(Symbol.Gain, card.Gains, BlockStyles.Body, availableBounds, canvas);
        availableBounds = DrawHorizontalLine(BlockStyles.Body.SpacingBefore, availableBounds, canvas);
        availableBounds = DrawGainOrCost(Symbol.Cost, card.Gains, BlockStyles.Body, availableBounds, canvas);
        availableBounds = DrawHorizontalLine(BlockStyles.Body.SpacingBefore, availableBounds, canvas);
    }

    private RectF DrawHorizontalLine(float spacingBefore, RectF availableBounds, ICanvas canvas)
    {
        float y = availableBounds.Top + spacingBefore;
        canvas.DrawLine(availableBounds.Left, y, availableBounds.Right, y);
        return RectF.FromLTRB(availableBounds.Left, y, availableBounds.Right, availableBounds.Bottom);
    }

    private RectF DrawGainOrCost(string direction, Item[] items, BlockStyle style, RectF availableBounds, ICanvas canvas)
    {
        const string EmDash = "\x2014";
        availableBounds = DrawText(direction, style, availableBounds, canvas);
        if (items.Length == 0)
            return DrawText(EmDash, style, availableBounds, canvas);
        string text = string.Join('\n', items.Select(Format));
        return DrawText(text, style, availableBounds, canvas);
    }

    private void DrawExclusive(RectF availableBounds, ICanvas canvas)
    {
        BlockStyle style = BlockStyles.Title;
        if (!style.BlockHeight.HasValue) throw new ArgumentException("BlockStyles.Title must have a fixed BlockHeight.");
        float size = style.BlockHeight.Value;
        RectF textBounds = RectF.FromLTRB(availableBounds.Right - size, availableBounds.Top, availableBounds.Right, availableBounds.Top + size);
        DrawText(Symbol.Exclusive, BlockStyles.Icons, textBounds, canvas);
    }

    private void DrawCategories(Category[] categories, RectF availableBounds, ICanvas canvas)
    {
        BlockStyle style = BlockStyles.Title;
        if (!style.BlockHeight.HasValue) throw new ArgumentException("BlockStyles.Title must have a fixed BlockHeight.");
        float size = style.BlockHeight.Value;
        RectF textBounds = new(availableBounds.Left, availableBounds.Top, size, size * categories.Length);
        string text = string.Join('\n', categories.Select(category => $"[{category}]".ToLower()));
        DrawText(text, BlockStyles.Icons, textBounds, canvas);
    }

    private RectF DrawAbandonmentCost(Item abandonmentCost, RectF availableBounds, ICanvas canvas)
    {
        BlockStyle style = BlockStyles.Body;
        GraphicsContext context = new() {
            Bounds = availableBounds,
            Canvas = canvas,
            Font = style.Font,
            FontSize = style.FontSize,
        };
        Block block = textParser.CreateBlock(Format(abandonmentCost));
        block.CalculateSize(context);
        float height = block.Bounds.Height;
        context.Bounds.Height = height;
        context.Bounds.Y = availableBounds.Bottom - height;
        block.CalculateLocation(context);
        block.Draw(context);
        return RectF.FromLTRB(availableBounds.Left, availableBounds.Top, availableBounds.Right, context.Bounds.Top - style.SpacingBefore);
    }

    private string Format(Item item)
    {
        StringBuilder text = new(Resource.MaxLength + 2 * 5 + 7);
        List<string> tokens = new(5);
        text.Append('|');
        if (item.D is not null) text.Append(item.D).Append('|');
        if (item.T.HasValue) tokens.Add($"{item.T}{Resource.T}");
        if (item.F.HasValue) tokens.Add($"{item.F}{Resource.F}");
        if (item.P.HasValue) tokens.Add($"{item.P}{Resource.P}");
        if (item.M.HasValue) tokens.Add($"{item.M}{Resource.M}");
        if (item.E.HasValue) tokens.Add($"{item.E}{Resource.E}");
        text.Append(string.Join(' ', tokens));
        text.Append('|');
        return text.ToString();
    }

    /// <inheritdoc cref="DrawTo(ICanvas, Background)"/>
    public void DrawTo(ICanvas canvas, Value card)
    {
        RectF bounds = InitialiseBounds();
        bounds = DrawText(card.Title, BlockStyles.Title, bounds, canvas);
        bounds = DrawText(card.Text, BlockStyles.Body, bounds, canvas);
    }

    private RectF InitialiseBounds() => RectF.FromLTRB(Margin, Margin, CardPng.Width - Margin, CardPng.Height - Margin);

    /// <summary>
    /// Draws formatted text to an area of the canvas.
    /// </summary>
    /// <param name="text">The formatted text to draw.</param>
    /// <param name="style">The style to draw with.</param>
    /// <param name="availableBounds">The available area to draw to.</param>
    /// <param name="canvas">The canvas to draw to.</param>
    /// <returns>The remaining available bounds.</returns>
    private RectF DrawText(string text, BlockStyle style, RectF availableBounds, ICanvas canvas)
    {
        availableBounds.Height = GetHeight(availableBounds.Y, style.BlockHeight);
        RectF textBounds = RectF.FromLTRB(availableBounds.Left, availableBounds.Top + style.SpacingBefore, availableBounds.Right, availableBounds.Bottom);
        if (style.BlockHeight.HasValue) textBounds.Height = style.BlockHeight.Value;
        GraphicsContext context = new() {
            Bounds = textBounds,
            Canvas = canvas,
            Font = style.Font,
            FontSize = style.FontSize,
        };
        Block block = textParser.CreateBlock(text);
        block.CalculateSize(context);
        block.CalculateLocation(context);
        block.Draw(context);
        return RectF.FromLTRB(availableBounds.Left, textBounds.Bottom, availableBounds.Right, availableBounds.Bottom);
    }

    private float GetHeight(float y, float? height) =>
        height.HasValue ? height.Value : CardPng.Height - Margin * 2 - y;

    private class BlockStyle
    {
        /// <summary>
        /// The font to use.
        /// </summary>
        public required IFont Font;

        /// <summary>
        /// The font size to use.
        /// </summary>
        public required float FontSize;

        /// <summary>
        /// The block's fixed height, or null to use the remaining space.
        /// </summary>
        public float? BlockHeight = null;

        /// <summary>
        /// The vertical spacing before this block.
        /// </summary>
        public float SpacingBefore = 0f;
    }
}
