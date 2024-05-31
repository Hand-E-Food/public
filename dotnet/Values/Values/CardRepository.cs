using Values.Model;

namespace Values;

/// <summary>
/// Provides access to all cards.
/// </summary>
public interface ICardRepository
{
    /// <summary>
    /// The player background cards.
    /// </summary>
    Background[] Backgrounds { get; }
    /// <summary>
    /// The decade cards.
    /// </summary>
    Decade[] Decades { get; }
    /// <summary>
    /// The lifestyle cards.
    /// </summary>
    Lifestyle[] Lifestyles { get; }
    /// <summary>
    /// The societal and personal value cards.
    /// </summary>
    Value[] Values { get; }
}

/// <summary>
/// Provides access to all cards in the game.
/// </summary>
public class CardRepository : ICardRepository
{
    public Background[] Backgrounds { get; } = [
        AverageBackground,
    ];
    private static readonly Background AverageBackground = new() {
        Title = "Average",
        Gains = [
            new() { D = "20s", P = 2, M = 2, E = 2, F = 2, T = 7 },
            new() { D = "30s", P = 1, M = 2, E = 2, F = 1, T = 7 },
            new() { D = "40s", P = 1, M = 1, E = 2, F = 0, T = 7 },
            new() { D = "50s", P = 0, M = 1, E = 1, F = 0, T = 7 },
            new() { D = "60s", P = 0, M = 0, E = 0, F = 0, T = 6 },
        ]
    };

    public Decade[] Decades { get; } = [
        new() {
            Title = "20!",
            Text =
                "Give each player a random background card.\n" +
                "Players each take the tokens listed next to \"20s\" on their card.\n" +
                "Place as many Value cards on the table as there are players, face up.\n" +
                "Give each player two Value cards, face down. Each player keeps one and discards the other.\n" +
                "Choose a player to go first.\n" +
                "Turn over three 20s cards."
        },
        IntermediateDecade("30!", "20s", "30s"),
        IntermediateDecade("40!", "30s", "40s"),
        IntermediateDecade("50!", "40s", "50s"),
        IntermediateDecade("60!", "50s", "60s"),
        new() {
            Title = "70!",
            Text =
                "Discard all remaining 60s cards.\n" +
                "All players calculate their scores:\n" +
                $"Earn 1 {Symbol.Point} for each {Resource.P}, {Resource.M} and {Resource.E} token you have. Double the {Symbol.Point}s of whichever token you have the least of.\n" +
                $"Earn {Symbol.Point}s from the Societal Values.\n" +
                $"Earn {Symbol.Point}s from you Personal Value.\n" +
                $"The player with the most {Symbol.Point}s wins."
        },
    ];
    /// <summary>
    /// Creates a decade card with default text.
    /// </summary>
    /// <param name="title">This decade's title.</param>
    /// <param name="previousDecade">The previous decade.</param>
    /// <param name="nextDecade">The next decade.</param>
    private static Decade IntermediateDecade(string title, string previousDecade, string nextDecade) => new() {
        Title = title,
        Text =
            $"Discard all remaining {previousDecade} cards. Turn over the same number of {nextDecade} cards.\n" +
            "All players, check your background and lifestyle cards. The resources you gain or spend may have changed.\n" +
            "If you gain more or less resources, collect or return tokens as necessary.\n" +
            "If you spend more or less resources, adjust where your resources are assigned.\n" +
            "If you cannot afford your lifestyle, you must abandon unaffordable lifestyle choices on your next turn.\n" +
            "Discard this card and skip the next player's turn so that a different player starts this decade.",
    };

    public Lifestyle[] Lifestyles { get; } = [
        Marriage("20s", "Socialite", Category.Status) with {
            Costs = [
                new() { E = 1, F = 2, T = 1 }
            ],
        },
    ];
    /// <summary>
    /// Creates a "Marriage" lifestyle card.
    /// </summary>
    /// <param name="decade">The decade in which this lifestyle can be chosen.</param>
    /// <param name="stereotype">The spouse's stereotype.</param>
    /// <param name="category">This marriage's category, aside from <see cref="Category.Family"/>.</param>
    /// <returns>A "Marriage" lifestyle card.</returns>
    private static Lifestyle Marriage(string decade, string stereotype, Category category)
    {
        return new() {
            Decade = decade,
            Exclusive = true,
            Title = "Marriage",
            Subtitle = stereotype,
            Categories = [ Category.Family, category ],
            Gains = [
                new() { P = 2, M = 2, E = 2, F = 1 },
            ],
            AbandonmentCost = new() { E = 1, F = 1 },
        };
    }

    public Value[] Values { get; } = [
        new() {
            Title = "Body",
            Text = 
                "Each current card:" + 
                $"{Symbol.Point} x {Resource.P} gained",
        },
        new() {
            Title = "Mind",
            Text = 
                "Each current card:" + 
                $"{Symbol.Point} x {Resource.M} gained",
        },
        new() {
            Title = "Soul",
            Text =
                "Each current card:" + 
                $"{Symbol.Point} x {Resource.E} gained",
        },
        new() {
            Title = "Beauty",
            Text =
                $"Each current {Category.Beauty} card:" +
                $"{Symbol.Point}{Symbol.Point}{Symbol.Point}",
        },
        new() {
            Title = "Career",
            Text =
                $"One current or abandoned {Category.Career} card:\n" +
                $"{Symbol.Point} x {Resource.P} {Resource.M} {Resource.E} gained",
        },
        new() {
            Title = "Charity",
            Text =
                $"Each current and abandoned {Category.Charity} card:\n" +
                $"{Symbol.Point}{Symbol.Point}",
        },
        new() {
            Title = "Creativity",
            Text =
                $"Each current and abandoned {Category.Creativity} card:" +
                $"{Symbol.Point}{Symbol.Point}",
        },
        new() {
            Title = "Faith",
            Text =
                $"Each current {Category.Faith} card:" +
                $"{Symbol.Point} x {Resource.E} gained",
        },
        new() {
            Title = "Family",
            Text =
                $"Count current {Category.Family} cards:" +
                $"|{Category.Family}|1|2|3|4|...|" +
                $"|{Symbol.Point}|1|3|6|10|+5|",
        },
        new() {
            Title = "Justice",
            Text =
                $"Each current {Category.Justice} card:" +
                $"{Symbol.Point}{Symbol.Point}{Symbol.Point}{Symbol.Point}",
        },
        new() {
            Title = "Pleasure",
            Text =
                $"Each current {Category.Pleasure} card:" +
                $"{Symbol.Point}{Symbol.Point}{Symbol.Point}{Symbol.Point}",
        },
        new() {
            Title = "Status",
            Text =
                $"Count players with the most {Category.Status} cards. These players gain {Symbol.Point}s:\n" +
                "||1|2|3|4|" +
                $"|{Symbol.Point}|9|4|3|2|",
        },
    ];
}
