namespace Values.Model;

/// <summary>
/// A group of resources that are gained or spent.
/// </summary>
public record Item
{
    /// <summary>
    /// The decade(s) this applies to.
    /// </summary>
    public string? D = null;
    /// <summary>
    /// Change in physical energy.
    /// </summary>
    public int? P = null;
    /// <summary>
    /// Change in mental energy.
    /// </summary>
    public int? M = null;
    /// <summary>
    /// Change in emotional energy.
    /// </summary>
    public int? E = null;
    /// <summary>
    /// Change in finances.
    /// </summary>
    public int? F = null;
    /// <summary>
    /// Chane in time availability.
    /// </summary>
    public int? T = null;
}
