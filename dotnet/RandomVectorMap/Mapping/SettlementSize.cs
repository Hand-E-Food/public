namespace RandomVectorMap.Mapping;

/// <summary>
/// Enumerates the possible settlement sizes.
/// </summary>
public enum SettlementSize
{
    /// <summary>
    /// No settlement is defined.
    /// </summary>
    Undefined,

    /// <summary>
    /// No settlement.
    /// </summary>
    None,

    /// <summary>
    /// A service station.
    /// </summary>
    Service,

    /// <summary>
    /// A homestead.
    /// </summary>
    Homestead,

    /// <summary>
    /// A sizable town.
    /// </summary>
    Town,

    /// <summary>
    /// A sprawling city.
    /// </summary>
    City,
}
