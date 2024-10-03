namespace Underground;

/// <summary>
/// Something that can be a cause or affected by a cause.
/// </summary>
public class Actor
{
    /// <summary>
    /// This actor's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// This actor's traits.
    /// </summary>
    public TraitCollection Traits { get; } = [];
}
