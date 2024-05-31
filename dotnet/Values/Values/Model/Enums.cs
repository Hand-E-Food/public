namespace Values.Model;

/// <summary>
/// Creates a new <see cref="StringEnum"/> object.
/// </summary>
/// <param name="Value">This enum's value.</param>
public abstract record StringEnum(string Value)
{
    public override string ToString() => Value;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    public static implicit operator string(StringEnum e) => e.ToString();
}

/// <summary>
/// A card's category.
/// </summary>
public sealed record Category : StringEnum
{
    public static readonly Category Beauty = new("[beauty]");
    public static readonly Category Career = new("[career]");
    public static readonly Category Charity = new("[charity]");
    public static readonly Category Creativity = new("[creativity]");
    public static readonly Category Faith = new("[faith]");
    public static readonly Category Family = new("[family]");
    public static readonly Category Justice = new("[justice]");
    public static readonly Category Pleasure = new("[pleasure]");
    public static readonly Category Status = new("[status]");

    private Category(string value) : base(value) {}
}

/// <summary>
/// A player resource.
/// </summary>
public sealed record Resource : StringEnum
{
    /// <summary>Time</summary>
    public static readonly Resource T = new("[time]");
    /// <summary>Financial mobility</summary>
    public static readonly Resource F = new("[financial]");
    /// <summary>Physical energy</summary>
    public static readonly Resource P = new("[physical]");
    /// <summary>Mental energy</summary>
    public static readonly Resource M = new("[mental]");
    /// <summary>Emotional energy</summary>
    public static readonly Resource E = new("[emotional]");

    /// <summary>The sum of the length of all strings in this enum.</summary>
    public static readonly int MaxLength = new[] {T, F, P, M, E}.Sum(text => text.ToString().Length);

    private Resource(string value) : base(value) {}
}

/// <summary>
/// Additional symbol.
/// </summary>
public sealed record Symbol : StringEnum
{
    /// <summary>Indicates what a lifestyle costs the player.</summary>
    public static readonly Symbol Cost = new("[cost]");
    /// <summary>Indicates a lifestlyle that is exclusive to similar lifestyles.</summary>
    public static readonly Symbol Exclusive = new("[exclusive]");
    /// <summary>Indicates the gains a lifestyle gives a player.</summary>
    public static readonly Symbol Gain = new("[gain]");
    /// <summary>Indicates a point for the player.</summary>
    public static readonly Symbol Point = new("[point]");

    private Symbol(string value) : base(value) {}
}
