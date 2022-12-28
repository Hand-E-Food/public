namespace PsiFi
{
    /// <summary>
    /// An ability.
    /// </summary>
    public interface Ability
    {
        /// <summary>
        /// True if this ability can currently be activated.
        /// False if this ability cannot currently be activated.
        /// </summary>
        bool CanActivateAbility { get; }

        /// <summary>
        /// This ability's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Activates this ability.
        /// </summary>
        /// <returns>A sequence of player interactions caused by activating this ability.</returns>
        IEnumerable<Interaction> ActivateAbility();
    }
}
