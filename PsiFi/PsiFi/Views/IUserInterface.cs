namespace PsiFi.Views
{
    /// <summary>
    /// A menu.
    /// </summary>
    /// <typeparam name="T">The type of response returned by the menu.</typeparam>
    interface IUserInterface<T>
    {
        /// <summary>
        /// Shows this menu.
        /// </summary>
        /// <returns>The response of this menu.</returns>
        T GetInput();
    }
}
